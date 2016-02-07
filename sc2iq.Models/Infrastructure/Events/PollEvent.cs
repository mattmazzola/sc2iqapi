using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Events
{
    public class PollEvent : TableEntity
    {
        public Guid Id { get; set; }

        public PollEvent()
        {
        }

        public PollEvent(string pollId)
        {
            PartitionKey = GetPartitionKey(pollId);
            RowKey = Guid.NewGuid().ToString();
        }

        public static string GetPartitionKey(string pollId)
        {
            return $"poll-{pollId}";
        }
    }
}
