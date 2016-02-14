using Microsoft.WindowsAzure.Storage.Table;
using sc2iq.Models.Infrastructure.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Events
{
    public class PollEvent : VersionedEvent
    {
        public Guid Id { get; set; }

        public PollEvent()
        {
        }


        public PollEventTableEntity ToTableEntity()
        {
            return new PollEventTableEntity(this.Id, this.SourceId);
        }
    }
}
