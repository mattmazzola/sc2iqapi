using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using sc2iq.Models.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.CommandHandler
{
    public abstract class CommandProcessor
    {
        private readonly Dictionary<Type, Action<ICommand>> handlers = new Dictionary<Type, Action<ICommand>>();

        public async Task ProcessCommand<TCommand>(BrokeredMessage message) where TCommand : ICommand
        {
            var type = typeof(TCommand).ToString();

            TCommand command;

            try
            {
                var json = message.GetBody<string>();
                command = JsonConvert.DeserializeObject<TCommand>(json);
            }
            catch (Exception e)
            {
                await message.DeadLetterAsync($"Message could not be deserialized as type: {type}", e.ToString());
                throw;
            }

            var handler = handlers[typeof(TCommand)];

            handler.Invoke(command);
        }

        protected void Handles<TCommand>(Func<TCommand, Task> handler) where TCommand : ICommand
        {
            this.handlers.Add(typeof(TCommand), command => handler((TCommand)command));
        }
    }
}
