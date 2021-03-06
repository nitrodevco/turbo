using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Players.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService(ILogger<IAuthenticationService> logger)
        {
        }
    }
}
