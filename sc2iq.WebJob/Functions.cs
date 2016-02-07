using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

using sc2iq.Models.Infrastructure.Commands;
using sc2iq.Models.Infrastructure.Repositories;
using sc2iq.Models.Infrastructure.Aggregates;

namespace sc2iq.WebJob
{
    public class Functions
    {
        public async static void ProcessPollsCommandsOnMessage([ServiceBusTrigger("polls/commands", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            var json = message.GetBody<string>();
            log.WriteLine(message);
            Console.WriteLine(json);

            if(message.ContentType.Equals("application/json"))
            {
                if (message.Label.Equals(typeof(PollCreateCommand).ToString()))
                {
                    PollCreateCommand pollCreateCommand = null;

                    try
                    {
                        pollCreateCommand = JsonConvert.DeserializeObject<PollCreateCommand>(json);
                    }
                    catch (Exception e)
                    {
                        await message.DeadLetterAsync($"Message could not be deserialized as type: {message.Label}", e.ToString());
                        throw;
                    }

                    if(!pollCreateCommand.IsValid())
                    {
                        throw new InvalidDataException("Command is invalid");
                    }

                    var poll = new Poll(pollCreateCommand.Title, pollCreateCommand.Description);
                    var repository = new PollsRepository();
                    repository.Save(poll);

                    await message.CompleteAsync();
                }
            }
        }
    }
}
