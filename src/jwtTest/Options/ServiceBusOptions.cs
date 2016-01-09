using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtTest.Options
{
    public class ServiceBusOptions
    {
        public string ServiceBusPrimaryKey { get; set; }
        public Messaging Messaging { get; set; }
    }

    public class Messaging
    {
        public string Namespace { get; set; }
    }
}
