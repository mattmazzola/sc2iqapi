using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.OptionsModel;
using sc2iqapi.Options;
using sc2iqapi.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace sc2iqapi.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        public TokenController(IOptions<Secrets> optionsAccessor)
        {
            Options = optionsAccessor.Options;
        }

        Secrets Options { get; }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OauthTokenData oauthTokenData)
        {
            var formUrlEncodedContent = $"grant_type=authorization_code&scope={oauthTokenData.Scope}&code={oauthTokenData.Code}&redirect_uri={oauthTokenData.RedirectUri}";

            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://us.battle.net/oauth/token");
            request.Content = new StringContent(formUrlEncodedContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            var basicAuth = $"{oauthTokenData.ClientId}:{Options.BattlenetClientSecret}";
            var bytes = Encoding.UTF8.GetBytes(basicAuth);
            var base64 = Convert.ToBase64String(bytes);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);

            return Json(json);
        }
    }
}
