using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

using sc2iq.Models.Infrastructure.Commands;
using sc2iq.Models.Infrastructure.Repositories;
using sc2iq.Models.Infrastructure.Aggregates;
using sc2iq.Models.Infrastructure.CommandHandler;

namespace sc2iq.WebJob
{
    public class Functions
    {
        public async static void ProcessPollsCommandsOnMessage([ServiceBusTrigger("polls/commands", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            if(message.ContentType.Equals("application/json"))
            {
                var pollCommandProcessor = new PollCommandProcessor();

                if (message.Label.Equals(typeof(PollCreateCommand).ToString()))
                {
                    await pollCommandProcessor.ProcessCommand<PollCreateCommand>(message);
                }
                else if (message.Label.Equals(typeof(PollVoteAddCommand).ToString()))
                {
                    await pollCommandProcessor.ProcessCommand<PollVoteAddCommand>(message);
                }
                else if (message.Label.Equals(typeof(PollVoteRemoveCommand).ToString()))
                {
                    await pollCommandProcessor.ProcessCommand<PollVoteRemoveCommand>(message);
                }

                await message.CompleteAsync();
            }
            else
            {
                await message.DeadLetterAsync($"Message has ContentType that is not understood.", $"Message ContentType must be application/json. Message's ContentType was: {message.ContentType}");
            }
        }

        public async static void ProcessQuestionsCommandsOnMessage([ServiceBusTrigger("questions/commands", "all", AccessRights.Listen)] BrokeredMessage message, TextWriter log)
        {
            var json = message.GetBody<string>();
            log.WriteLine(message);
            Console.WriteLine(json);

            if (message.ContentType.Equals("application/json"))
            {
                if (message.Label.Equals(typeof(QuestionCreateCommand).ToString()))
                {
                    QuestionCreateCommand questionCreateCommand = null;

                    try
                    {
                        questionCreateCommand = JsonConvert.DeserializeObject<QuestionCreateCommand>(json);
                    }
                    catch (Exception e)
                    {
                        await message.DeadLetterAsync($"Message could not be deserialized as type: {message.Label}", e.ToString());
                        throw;
                    }

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

                    await message.CompleteAsync();
                }
                else if (message.Label.Equals(typeof(QuestionPublishCommand).ToString()))
                {
                    PollVoteAddCommand pollVoteAddCommand = null;

                    try
                    {
                        pollVoteAddCommand = JsonConvert.DeserializeObject<PollVoteAddCommand>(json);
                    }
                    catch (Exception e)
                    {
                        await message.DeadLetterAsync($"Message could not be deserialized as type: {message.Label}", e.ToString());
                        throw;
                    }

                    var repository = new PollsRepository();
                    var poll = await repository.Find(pollVoteAddCommand.PollId);

                    if (poll == null)
                    {
                        throw new Exception($"Could not find poll with id: {pollVoteAddCommand.PollId}");
                    }

                    poll.VoteAdd();
                    repository.Save(poll);

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
