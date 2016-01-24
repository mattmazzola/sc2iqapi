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

        public void Find(string id)
        {

        }

        public async void Save(Poll poll)
        {
            // Serialize object
            var aggregateJson = JsonConvert.SerializeObject(poll);
            
            // Save aggregate in cache
            var cache = Connection.GetDatabase();
            await cache.SetAddAsync(poll.Id.ToString(), aggregateJson);

            // Save aggregate in event store.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("events");
            await table.CreateIfNotExistsAsync();

            var pollCreatedEvent = new PollCreatedEvent()
            {
                Id = poll.Id,
                Title = poll.Title
            };
            await table.ExecuteAsync(TableOperation.Insert(pollCreatedEvent));

            // Send message to event topic that poll was saved.
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollseventssend", ConfigurationManager.AppSettings["pollseventsend"]);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["namespace"], string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/events");

            var eventJson = JsonConvert.SerializeObject(pollCreatedEvent);
            var message = new BrokeredMessage(eventJson);
            message.ContentType = "application/json";
            message.Label = pollCreatedEvent.GetType().ToString();

            await topicClient.SendAsync(message);
        }
    }
}
