using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using jwtTest.Models;
using jwtTest.Commands;
using jwtTest.Infrastructure.Messaging;
using jwtTest.Options;
using Microsoft.Extensions.OptionsModel;
using System.Net;
using System.Security.Cryptography;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

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
            var pollCreateCommand = new PollCreateCommand()
            {
                Title = poll.Title,
                Description = poll.Description
            };

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("pollscommandssend", serviceBusOptions.ServiceBusPrimaryKey);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("polls/commands");

            var brokeredMessage = new BrokeredMessage(JsonConvert.SerializeObject(pollCreateCommand));
            await topicClient.SendAsync(brokeredMessage);

            return Created(string.Empty, null);
        }
    }
}
