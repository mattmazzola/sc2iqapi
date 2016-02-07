using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Commands
{
    public class PollVoteRemoveCommand
    {
        public Guid Id { get; private set; }
        public string PollId { get; private set; }

        public PollVoteRemoveCommand(string pollId)
        {
            Id = Guid.NewGuid();
            PollId = pollId;
        }
    }
}
