using sc2iq.Models.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Commands
{
    public class QuestionPublishCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
