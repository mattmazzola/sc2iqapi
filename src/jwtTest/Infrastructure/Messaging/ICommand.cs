using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtTest.Infrastructure.Messaging
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
