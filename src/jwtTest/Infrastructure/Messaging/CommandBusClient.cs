using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace jwtTest.Infrastructure.Messaging
{
    public class CommandBusClient : ICommandBus
    {


        public void Send(Envelope<ICommand> command)
        {
            var webClient = new HttpClient();
        }
        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {

        }
    }
}
