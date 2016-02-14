using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using jwtTest.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.OptionsModel;
using jwtTest.Options;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace jwtTest.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly TokenAuthOptions tokenOptions;

        public TokenController(TokenAuthOptions tokenOptions, IOptions<Secrets> optionsAccessor)
        {
            this.tokenOptions = tokenOptions;
            Options = optionsAccessor.Value;
            //this.bearerOptions = options.Value;
            //this.signingCredentials = signingCredentials;
        }

        Secrets Options { get; }

        /// <summary>
        /// Check if currently authenticated. Will throw an exception of some sort which shoudl be caught by a general
        /// exception handler and returned to the user as a 401, if not authenticated. Will return a fresh token if
        /// the user is authenticated, which will reset the expiry.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Bearer")]
        public dynamic Get()
        {
            /* 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            THIS METHOD SHOULD BE REMOVED IN PRODUCTION USE-CASES - IT ALLOWS A USER WITH 
            A VALID TOKEN TO REMAIN LOGGED IN FOREVER, WITH NO WAY OF EVER EXPIRING THEIR
            RIGHT TO USE THE APPLICATION.
            Refresh Tokens (see https://auth0.com/docs/refresh-token) should be used to 
            retrieve new tokens. 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            ******* WARNING WARNING WARNING ****** 
            */
            bool authenticated = false;
            string user = null;
            int entityId = -1;
            string token = null;
            DateTime? tokenExpires = default(DateTime?);

            var currentUser = HttpContext.User;
            if (currentUser != null)
            {
                authenticated = currentUser.Identity.IsAuthenticated;
                if (authenticated)
                {
                    user = currentUser.Identity.Name;
                    foreach (Claim c in currentUser.Claims) if (c.Type == "EntityID") entityId = Convert.ToInt32(c.Value);
                    tokenExpires = DateTime.UtcNow.AddMinutes(2);
                    token = GetToken(currentUser.Identity.Name, tokenExpires);
                }
            }
            return new { authenticated = authenticated, user = user, entityId = entityId, token = token, tokenExpires = tokenExpires };
        }

        /// <summary>
        /// Request a new token for a given username/password pair.
        /// </summary>
        /// <param name="oauthTokenData"></param>
        /// <returns></returns>
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
            var battleNetAuthentication = JsonConvert.DeserializeObject<BattleNetAuthentication>(jsonString);

            var expires = DateTime.UtcNow.AddSeconds(battleNetAuthentication.Expires);
            var token = GetToken("FakeUserName", expires);

            return Json(new {
                sc2iqToken = token,
                battletNetToken = battleNetAuthentication.AccessToken,
                expires
            });
        }

        private string GetToken(string user, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the user which is being authenticated.
            // For now, just creating a simple generic identity.
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth2"), new[] { new Claim("EntityID", "1", ClaimValueTypes.Integer) });

            var securityToken = handler.CreateToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                signingCredentials: tokenOptions.SigningCredentials,
                subject: identity,
                expires: expires
                );
            return handler.WriteToken(securityToken);
        }

    }
}
