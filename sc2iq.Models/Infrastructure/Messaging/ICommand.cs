using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Messaging
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
