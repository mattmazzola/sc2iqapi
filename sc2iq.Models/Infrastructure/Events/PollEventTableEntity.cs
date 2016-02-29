using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Events
{
    public class PollEventTableEntity : TableEntity
    {
        public PollEventTableEntity() : base()
        {
        }

        public PollEventTableEntity(Guid Id, Guid SourceId) : base()
        {
            this.PartitionKey = GetPartitionKey(Id.ToString());
            this.RowKey = SourceId.ToString();
        }

        public static string GetPartitionKey(string id)
        {
            return $"poll-{id}";
        }
    }
}
