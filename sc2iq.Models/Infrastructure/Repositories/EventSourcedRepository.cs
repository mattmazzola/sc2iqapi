using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Repositories
{
    public class EventSourcedRepository<T, E> where E : TableEntity, new()
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

        public async Task<T> Find(string id)
        {
            // Find aggregate in cache
            //var cache = Connection.GetDatabase();
            //string json = await cache.StringGetAsync(id);

            string json = string.Empty;

            if (string.IsNullOrEmpty(json))
            {
                // Find aggregate by replaying events
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("events");

                var query = new TableQuery<E>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, E.GetPartitionKey(id)));

                var events = table.ExecuteQuery(query);
            }

            var aggregate = JsonConvert.DeserializeObject<T>(json);

            return aggregate;
        }
    }
}
