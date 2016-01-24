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
        public static void ProcessPollsCommandsOnMessage([ServiceBusTrigger("polls/commands", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            var json = message.GetBody<string>();

            Console.WriteLine($"ContentType: {message.ContentType}");
            Console.WriteLine($"Label: {message.Label}");

            log.WriteLine(message);
            Console.WriteLine(json);

            if(message.ContentType.Equals("application/json"))
            {
                if (message.Label.Equals(typeof(PollCreateCommand).ToString()))
                {
                    var pollCreateCommand = JsonConvert.DeserializeObject<PollCreateCommand>(json);
                    Console.WriteLine(pollCreateCommand);
                    Console.WriteLine($"Title: {pollCreateCommand.Title}");

                    var poll = new Poll(pollCreateCommand.Title, pollCreateCommand.Description);
                    var repository = new PollsRepository();
                    repository.Save(poll);

                }
            }
        }
    }
}
