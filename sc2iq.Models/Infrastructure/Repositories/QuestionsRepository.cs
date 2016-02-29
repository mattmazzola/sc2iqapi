using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using sc2iq.Models.Infrastructure.Aggregates;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Repositories
{
    public class QuestionsRepository
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

        public async Task<Question> Find(string questionId)
        {
            var question = new Question("q", "a1", "a2", "a3", "a4", 0, DateTimeOffset.UtcNow, "me", 3, QuestionState.Pending, new List<string>() { "tag1", "tag2" });
            return await Task.FromResult(question);
        }

        public async void Save(Question question)
        {
            // Serialize object
            var aggregateJson = JsonConvert.SerializeObject(question);

            // Save aggregate in cache
            var cache = Connection.GetDatabase();
            await cache.StringSetAsync(question.Id.ToString(), aggregateJson);

            // Save aggregate in event store.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("events");
            await table.CreateIfNotExistsAsync();

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("questionseventssend", ConfigurationManager.AppSettings["questionsseventsend"]);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["namespace"], string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("questions/events");

            var messages = question.Events.Select(@event =>
            {
                var eventJson = JsonConvert.SerializeObject(@event);
                var message = new BrokeredMessage(eventJson);
                message.ContentType = "application/json";
                message.Label = @event.GetType().ToString();
                return message;
            });

            await topicClient.SendBatchAsync(messages);
        }
    }
}
