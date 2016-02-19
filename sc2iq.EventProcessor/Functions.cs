using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

using sc2iq.Models.Infrastructure.Events;
using sc2iq.Models.Infrastructure.Repositories;
using Microsoft.Azure.Documents.Client;
using sc2iq.Models.Infrastructure.Documents;

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
            else
            {
                await message.DeadLetterAsync($"Message has ContentType that is not understood.", $"Message ContentType must be application/json. Message's ContentType was: {message.ContentType}");
            }
        }

        public async static void ProcessQuestionsEventsOnMessage([ServiceBusTrigger("questions/events", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            var json = message.GetBody<string>();
            log.WriteLine(message);
            Console.WriteLine(json);

            if (message.ContentType.Equals("application/json"))
            {
                if (message.Label.Equals(typeof(QuestionCreatedEvent).ToString()))
                {
                    QuestionCreatedEvent questionCreatedEvent = null;

                    try
                    {
                        questionCreatedEvent = JsonConvert.DeserializeObject<QuestionCreatedEvent>(json);

                        var question = new Question()
                        {
                            Id = questionCreatedEvent.SourceId.ToString(),
                            Q = questionCreatedEvent.Q,
                            A1 = questionCreatedEvent.A1,
                            A2 = questionCreatedEvent.A2,
                            A3 = questionCreatedEvent.A3,
                            A4 = questionCreatedEvent.A4,
                            CorrectAnswerIndex = questionCreatedEvent.CorrectAnswerIndex,
                            Created = questionCreatedEvent.Created,
                            CreatedBy = questionCreatedEvent.CreatedBy,
                            Difficulty = questionCreatedEvent.Difficulty,
                            State = (Models.Infrastructure.Documents.QuestionState)questionCreatedEvent.State,
                            Tags = questionCreatedEvent.Tags
                        };

                        var documentDbClient = new DocumentClient(new Uri("https://sc2iq.documents.azure.com:443/"), "B8G2ra4iEr3rR7M9aWkqtUNtrkkcKt89fgvuG22+NUANVB4Ly1MZB3sEm6JUOmhjN2iGpzknKW6TaR447cSwHQ==");
                        var documentsLink = UriFactory.CreateDocumentCollectionUri("sc2iq", "questions");
                        await documentDbClient.CreateDocumentAsync(documentsLink, question);
                    }
                    catch (Exception e)
                    {
                        await message.DeadLetterAsync($"Message could not be deserialized as type: {message.Label}", e.ToString());
                        throw;
                    }

                    await message.CompleteAsync();
                }
            }
            else
            {
                await message.DeadLetterAsync($"Message has ContentType that is not understood.", $"Message ContentType must be application/json. Message's ContentType was: {message.ContentType}");
            }
        }
    }
}
