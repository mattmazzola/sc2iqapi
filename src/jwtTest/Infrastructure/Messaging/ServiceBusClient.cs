using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace jwtTest.Infrastructure.Messaging
{
    public class ServiceBusClient
    {
        private readonly string baseAddress;
        //private readonly string serviceNamespace;
        //private readonly string baseAddress;
        private readonly string token;
        private const string sbHostName = "servicebus.windows.net";

        public ServiceBusClient(string serviceNamespace, string policyName, string key)
        {
            this.baseAddress = $"https://{serviceNamespace}.{sbHostName}";
            this.token = CreateSasKey(policyName, key);
        }

        public async Task Send(Envelope<ICommand> command)
        {
            var topicName = "polls/commands";

            var fullAddress = $"{this.baseAddress}/{topicName}/messages?timeout=60&api-version=2013-08 ";
            Console.WriteLine("\nSending message {0} - to address {1}", command, fullAddress);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, fullAddress);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", this.token);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            await httpClient.SendAsync(httpRequestMessage);
        }

        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {

        }

        private string CreateSasKey(string keyName, string key)
        {
            TimeSpan fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)fromEpochStart.TotalSeconds + 3600);
            var stringToSign = WebUtility.UrlEncode(baseAddress) + "\n" + expiry;
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = $"sr={WebUtility.UrlEncode(baseAddress)}&sig={WebUtility.UrlEncode(signature)}&se={expiry}&skn={keyName}";

            return sasToken;
        }
    }
}
