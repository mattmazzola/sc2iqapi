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
using sc2iq.Models.Infrastructure.Aggregates;

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

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.serviceBusOptions);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Poll poll)
        {
            var pollCreateCommand = new PollCreateCommand(poll)
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
    }
}
