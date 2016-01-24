﻿using sc2iq.Models.Infrastructure.Aggregates;
using sc2iq.Models.Infrastructure.Messaging;
using System;

namespace sc2iq.Models.Infrastructure.Commands
{
    public class PollCreateCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PollCreateCommand(Poll poll)
        {
            Id = Guid.NewGuid();
        }
    }
}
