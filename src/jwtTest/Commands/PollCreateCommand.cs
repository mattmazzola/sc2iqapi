using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwtTest.Infrastructure.Messaging;

namespace jwtTest.Commands
{
    public class PollCreateCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
