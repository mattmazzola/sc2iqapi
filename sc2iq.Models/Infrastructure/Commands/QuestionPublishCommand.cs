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
        public string QuestionId { get; set; }

        public QuestionPublishCommand(string questionId)
        {
            Id = Guid.NewGuid();
            QuestionId = questionId;
        }
    }
}
