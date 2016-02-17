using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using jwtTest.Options;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Net;
using jwtTest.Models;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Newtonsoft.Json;
using sc2iq.Models.Infrastructure.Commands;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace jwtTest.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        private readonly ServiceBusOptions serviceBusOptions;
        private readonly DocumentClient documentDbClient;
        private readonly Uri documentsLink;

        public QuestionsController(IOptions<ServiceBusOptions> serviceBusOptions)
        {
            this.serviceBusOptions = serviceBusOptions.Value;
            this.documentDbClient = new DocumentClient(new Uri(this.serviceBusOptions.DocumentDb.EndpointUri), this.serviceBusOptions.DocumentDb.AuthorizationKey);
            this.documentsLink = UriFactory.CreateDocumentCollectionUri(this.serviceBusOptions.DocumentDb.DatabaseName, this.serviceBusOptions.DocumentDb.QuestionsCollection);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var questions = documentDbClient.CreateDocumentQuery<Question>(documentsLink).AsEnumerable().ToList();

            return Json(questions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var documentLink = UriFactory.CreateDocumentUri(serviceBusOptions.DocumentDb.DatabaseName, serviceBusOptions.DocumentDb.QuestionsCollection, id);
            ResourceResponse<Document> response;

            try
            {
                response = await documentDbClient.ReadDocumentAsync(documentLink);
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == "NotFound")
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.NotFound);
                }

                throw;
            }

            var question = (Question)(dynamic)response.Resource;

            return Json(question);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Question question)
        {
            // Enforce defaults for new item
            var questionCreateCommand = new QuestionCreateCommand("fakeUserId")
            {
                Q = question.Q,
                A1 = question.A1,
                A2 = question.A2,
                A3 = question.A3,
                A4 = question.A4
            };

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("questionscommandssend", serviceBusOptions.questionscommandssend);
            var messageFactory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusOptions.Messaging.Namespace, string.Empty), tokenProvider);
            var topicClient = messageFactory.CreateTopicClient("questions/commands");

            var json = JsonConvert.SerializeObject(questionCreateCommand);
            var message = new BrokeredMessage(json);
            message.ContentType = "application/json";
            message.Label = questionCreateCommand.GetType().ToString();

            await topicClient.SendAsync(message);

            return Created(string.Empty, null);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Question value)
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var documentLink = UriFactory.CreateDocumentUri(serviceBusOptions.DocumentDb.DatabaseName, serviceBusOptions.DocumentDb.QuestionsCollection, id);

            try
            {
                await documentDbClient.DeleteDocumentAsync(documentLink);
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == "NotFound")
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.NotFound);
                }

                throw;
            }

            return new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
        }
    }
}
