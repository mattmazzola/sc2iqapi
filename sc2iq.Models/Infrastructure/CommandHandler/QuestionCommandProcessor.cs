using Microsoft.ServiceBus.Messaging;
using sc2iq.Models.Infrastructure.Aggregates;
using sc2iq.Models.Infrastructure.Commands;
using sc2iq.Models.Infrastructure.Messaging;
using sc2iq.Models.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.CommandHandler
{
    public class QuestionCommandProcessor : CommandProcessor
    {
        private readonly Dictionary<Type, Action<ICommand>> handlers = new Dictionary<Type, Action<ICommand>>();

        public QuestionCommandProcessor()
        {
            this.Handles<QuestionCreateCommand>(this.OnCreate);
            this.Handles<QuestionPublishCommand>(this.OnPublish);
        }

        private async Task OnCreate(QuestionCreateCommand questionCreateCommand)
        {
            var question = new Question(
                        questionCreateCommand.Q,
                        questionCreateCommand.A1,
                        questionCreateCommand.A2,
                        questionCreateCommand.A3,
                        questionCreateCommand.A4,
                        questionCreateCommand.CorrectAnswerIndex,
                        questionCreateCommand.Created,
                        questionCreateCommand.CreatedBy,
                        questionCreateCommand.Difficulty,
                        (Models.Infrastructure.Aggregates.QuestionState)questionCreateCommand.State,
                        questionCreateCommand.Tags
                    );
            var repository = new QuestionsRepository();
            repository.Save(question);

            await Task.Delay(0);
        }

        private async Task OnPublish(QuestionPublishCommand questionPublishCommand)
        {
            var repository = new QuestionsRepository();
            var question = await repository.Find(questionPublishCommand.QuestionId);
            question.Publish();
            repository.Save(question);
        }
    }
}
