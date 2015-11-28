using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class OauthTokenData
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string GrantType { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
    }
}
