using FireSharp;
using FireSharp.Config;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using sc2iq.Models.Infrastructure.Aggregates;
using sc2iq.Models.Infrastructure.Events;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Repositories
{
    public class PollsRepository
    {
        static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var connectionString = ConfigurationManager.AppSettings["RedisConnectionString"];
            return ConnectionMultiplexer.Connect(connectionString);
        });

        static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }


        public async Task<Poll> Find(string id)
        {
            // Find aggregate in cache
            var cache = Connection.GetDatabase();
            string json = await cache.StringGetAsync(id);

            if(string.IsNullOrEmpty(json))
            {
                // Find aggregate by replaying events
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("events");

                var query = new TableQuery<PollEvent>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PollEvent.GetPartitionKey(id)));

                var events = table.ExecuteQuery(query);
            }

            var poll = JsonConvert.DeserializeObject<Poll>(json);

            return poll;
        }

        public async void Save(Poll poll)
        {
            // Serialize object
            var aggregateJson = JsonConvert.SerializeObject(poll);

            // Save aggregate in cache
            var cache = Connection.GetDatabase();
            await cache.StringSetAsync(poll.Id.ToString(), aggregateJson);

            // Save aggregate in event store.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("events");
            await table.CreateIfNotExistsAsync();

            PollEvent pollEvent;

            if(poll.Votes == 0)
            {
                pollEvent = new PollCreatedEvent(poll.Id.ToString())
                {
                    Title = poll.Title
                };
            }
            else
            {
                pollEvent = new PollVoteAddedEvent(poll.Id.ToString());
            }

            await table.ExecuteAsync(TableOperation.Insert(pollEvent));

            // Save updated aggregate to views.
            var firebaseUrl = "https://sc2iq.firebaseio.com/";
            var firebaseAuthId = "sc2iqapi";
            var firebaseSecret = "7YT5WAgHiMsk5xPAB2oO2l4xKSMznoFQvuYiPgws";
            var now = DateTime.UtcNow;
            var expiration = now.AddDays(1);
            Console.WriteLine($"expiration: {expiration}");

            var authPayload = new Dictionary<string, object>()
            {
                { "uid", firebaseAuthId }
            };
            var tokenOptions = new Firebase.TokenOptions(expires: expiration);
            var tokenGenerator = new Firebase.TokenGenerator(firebaseSecret);
            var token = tokenGenerator.CreateToken(authPayload, tokenOptions);

            var firebaseConfig = new FirebaseConfig()
            {
                AuthSecret = token,
                BasePath = firebaseUrl
            };

            var firebaseClient = new FirebaseClient(firebaseConfig);
            await firebaseClient.PushAsync("polls", poll);

            // Send message to event topic that poll was saved.
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollseventssend", ConfigurationManager.AppSettings["pollseventsend"]);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["namespace"], string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/events");

            var eventJson = JsonConvert.SerializeObject(pollEvent);
            var message = new BrokeredMessage(eventJson);
            message.ContentType = "application/json";
            message.Label = pollEvent.GetType().ToString();

            await topicClient.SendAsync(message);
        }
    }
}
