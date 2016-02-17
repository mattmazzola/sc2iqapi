using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net;
using jwtTest.Models;
using jwtTest.Options;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace jwtTest.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ServiceBusOptions serviceBusOptions;
        private readonly DocumentClient documentDbClient;
        private readonly Uri documentsLink;

        public UsersController(IOptions<ServiceBusOptions> serviceBusOptions)
        {
            this.serviceBusOptions = serviceBusOptions.Value;
            this.documentDbClient = new DocumentClient(new Uri(this.serviceBusOptions.DocumentDb.EndpointUri), this.serviceBusOptions.DocumentDb.AuthorizationKey);
            this.documentsLink = UriFactory.CreateDocumentCollectionUri(this.serviceBusOptions.DocumentDb.DatabaseName, this.serviceBusOptions.DocumentDb.UsersCollection);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = documentDbClient.CreateDocumentQuery<Models.User>(documentsLink).AsEnumerable().ToList();

            return Json(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var documentLink = UriFactory.CreateDocumentUri(serviceBusOptions.DocumentDb.DatabaseName, serviceBusOptions.DocumentDb.UsersCollection, id);
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

            var user = (Models.User)(dynamic)response.Resource;

            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Models.User user)
        {
            // Enforce defaults for new user
            user.PointsEarned = 0;
            user.PointsSpent = 0;
            user.Reputation = 0;
            user.Role = UserRole.User;
            user.Created = DateTimeOffset.UtcNow;

            await documentDbClient.CreateDocumentAsync(documentsLink, user);

            return Json(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Models.User user)
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var documentLink = UriFactory.CreateDocumentUri(serviceBusOptions.DocumentDb.DatabaseName, serviceBusOptions.DocumentDb.UsersCollection, id);

            try
            {
                await documentDbClient.DeleteDocumentAsync(documentLink);
            }
            catch(DocumentClientException e)
            {
                if(e.Error.Code == "NotFound")
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.NotFound);
                }

                throw;
            }

            return new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
        }
    }
}
