using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

using sc2iq.Models.Infrastructure.Events;
using sc2iq.Models.Infrastructure.Repositories;

namespace sc2iq.EventProcessor
{
    public class Functions
    {
        public async static void ProcessPollsEventsOnMessage([ServiceBusTrigger("polls/events", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            var json = message.GetBody<string>();
            log.WriteLine(message);
            Console.WriteLine(json);

            if (message.ContentType.Equals("application/json"))
            {
                if (message.Label.Equals(typeof(PollCreatedEvent).ToString()))
                {
                    PollCreatedEvent pollCreatedEvent = null;

                    try
                    {
                        pollCreatedEvent = JsonConvert.DeserializeObject<PollCreatedEvent>(json);
                    }
                    catch(Exception e)
                    {
                        await message.DeadLetterAsync($"Message could not be deserialized as type: {message.Label}", e.ToString());
                        throw;
                    }

                    await message.CompleteAsync();
                }
            }
        }
    }
}
