using Microsoft.WindowsAzure.Storage.Table;
using sc2iq.Models.Infrastructure.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Events
{
    public class PollCreatedEvent : PollEvent
    {
        public string Title { get; set; }

        public PollCreatedEvent(string pollId) : base(pollId)
        {
        }
    }
}
