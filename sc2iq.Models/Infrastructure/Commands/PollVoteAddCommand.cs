using sc2iq.Models.Infrastructure.Messaging;
using System;

namespace sc2iq.Models.Infrastructure.Commands
{
    public class PollVoteAddCommand : ICommand
    {
        public Guid Id { get; private set; }
        public string PollId { get; private set; }

        public PollVoteAddCommand(string pollId)
        {
            Id = Guid.NewGuid();
            PollId = pollId;
        }
    }
}
