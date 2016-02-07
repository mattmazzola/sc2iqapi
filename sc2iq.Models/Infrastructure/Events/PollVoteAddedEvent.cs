using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Events
{
    public class PollVoteAddedEvent : PollEvent
    {
        public PollVoteAddedEvent(string pollId) : base(pollId)
        {
        }
    }
}
