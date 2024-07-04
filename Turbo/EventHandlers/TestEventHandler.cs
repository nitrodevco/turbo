using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.EventHandlers;
using Turbo.Core.Events;
using Turbo.Events.Game.Rooms.Furniture;
using Turbo.Events.Game.Security;
using Turbo.Packets.Outgoing.Navigator;

namespace Turbo.EventHandlers
{
    public class TestEventHandler : ITestEventHandler
    {
        private readonly ITurboEventHub _eventHub;

        public TestEventHandler(ITurboEventHub eventHub)
        {
            _eventHub = eventHub;

            _eventHub.Subscribe<RemoveFloorFurnitureEvent>(this, OnRemoveFloorFurnitureEvent);
        }

        public void OnRemoveFloorFurnitureEvent(RemoveFloorFurnitureEvent message)
        {
            //message.Cancel();
        }
        
    }
}