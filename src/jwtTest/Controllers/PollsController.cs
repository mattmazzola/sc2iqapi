using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using jwtTest.Options;
using Microsoft.Extensions.OptionsModel;
using System.Net;
using System.Security.Cryptography;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using sc2iq.Models.Infrastructure.Commands;
using jwtTest.Models;
using sc2iq.Models.Infrastructure.Messaging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace jwtTest.Controllers
{
    [Route("api/[controller]")]
    public class PollsController : Controller
    {
        private readonly ServiceBusOptions serviceBusOptions;
        //private ICommandBus commandBus;

        public PollsController(IOptions<ServiceBusOptions> serviceBusOptions)
        {
            this.serviceBusOptions = serviceBusOptions.Value;
            //this.commandBus = commandBus;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(serviceBusOptions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Poll poll)
        {
            var pollCreateCommand = new PollCreateCommand()
            {
                Title = poll.Title,
                Description = poll.Description
            };

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollscommandssend", serviceBusOptions.pollscommandssend);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/commands");

            var json = JsonConvert.SerializeObject(pollCreateCommand);
            var message = new BrokeredMessage(json);
            message.ContentType = "application/json";
            message.Label = pollCreateCommand.GetType().ToString();

            await topicClient.SendAsync(message);

            return Created(string.Empty, null);
        }

        [HttpPatch("{id}/addvote")]
        public async Task<IActionResult> PatchVoteAdd(string id)
        {
            var pollVoteAddCommand = new PollVoteAddCommand(id);

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollscommandssend", serviceBusOptions.pollscommandssend);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/commands");

            var json = JsonConvert.SerializeObject(pollVoteAddCommand);
            var message = new BrokeredMessage(json);
            message.ContentType = "application/json";
            message.Label = pollVoteAddCommand.GetType().ToString();

            await topicClient.SendAsync(message);

            return new HttpStatusCodeResult((int)HttpStatusCode.Accepted);
        }

        [HttpPatch("{id}/removevote")]
        public async Task<IActionResult> PatchVoteRemove(string id)
        {
            var pollVoteRemoveCommand = new PollVoteRemoveCommand(id);

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollscommandssend", serviceBusOptions.pollscommandssend);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/commands");

            var json = JsonConvert.SerializeObject(pollVoteRemoveCommand);
            var message = new BrokeredMessage(json);
            message.ContentType = "application/json";
            message.Label = pollVoteRemoveCommand.GetType().ToString();

            await topicClient.SendAsync(message);

            return new HttpStatusCodeResult((int)HttpStatusCode.Accepted);
        }

        private async Task SendCommand(ICommand command)
        {
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollscommandssend", serviceBusOptions.pollscommandssend);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/commands");

            var json = JsonConvert.SerializeObject(command);
            var message = new BrokeredMessage(json);
            message.ContentType = "application/json";
            message.Label = command.GetType().ToString();

            await topicClient.SendAsync(message);
        }
    }
}
