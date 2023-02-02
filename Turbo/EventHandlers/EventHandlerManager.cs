using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.EventHandlers;

namespace Turbo.EventHandlers
{
    public class EventHandlerManager : IEventHandlerManager
    {
        private readonly ITestEventHandler _testEventHandler;

        public EventHandlerManager(
            ITestEventHandler testEventHandler)
        {
            _testEventHandler = testEventHandler;
        }
    }
}