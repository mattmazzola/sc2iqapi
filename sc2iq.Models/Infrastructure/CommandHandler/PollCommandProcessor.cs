using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
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
    public class PollCommandProcessor : CommandProcessor
    {
        private readonly Dictionary<Type, Action<ICommand>> handlers = new Dictionary<Type, Action<ICommand>>();

        public PollCommandProcessor()
        {
            this.Handles<PollCreateCommand>(this.OnCreate);
            this.Handles<PollVoteAddCommand>(this.OnVoteAdd);
            this.Handles<PollVoteRemoveCommand>(this.OnVoteRemove);
        }

        private async Task OnCreate(PollCreateCommand pollCreateCommand)
        {
            var poll = new Poll(pollCreateCommand.Title, pollCreateCommand.Description);
            var repository = new PollsRepository();
            repository.Save(poll);
            await Task.Delay(0);
        }

        private async Task OnVoteAdd(PollVoteAddCommand pollVoteAddCommand)
        {
            var repository = new PollsRepository();
            var poll = await repository.Find(pollVoteAddCommand.PollId);
            poll.VoteAdd();
            repository.Save(poll);
        }
        private async Task OnVoteRemove(PollVoteRemoveCommand pollVoteRemoveCommand)
        {
            var repository = new PollsRepository();
            var poll = await repository.Find(pollVoteRemoveCommand.PollId);
            poll.VoteRemove();
            repository.Save(poll);
        }
    }
}
