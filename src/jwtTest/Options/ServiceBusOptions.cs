using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtTest.Options
{
    public class ServiceBusOptions
    {
        public string pollscommandssend { get; set; }
        public string questionscommandssend { get; set; }
        public Messaging Messaging { get; set; }

        public DocumentDb DocumentDb { get; set; }
    }

    public class DocumentDb
    {
        public string EndpointUri { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollection { get; set; }
        public string QuestionsCollection { get; set; }
    }

    public class Messaging
    {
        public string Namespace { get; set; }
    }

    public class Secrets
    {
        public string BattlenetClientSecret { get; set; }
    }
}
