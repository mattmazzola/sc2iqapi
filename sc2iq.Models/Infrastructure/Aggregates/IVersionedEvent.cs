using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Aggregates
{
    public interface IVersionedEvent : IEvent
    {
        int Version { get; }
    }
}
