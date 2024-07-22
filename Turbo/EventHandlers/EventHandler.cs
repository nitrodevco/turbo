using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.EventHandlers;
using Turbo.Core.Events;
using Turbo.Events.Game.Rooms.Furniture;

namespace Turbo.EventHandlers
{
    public class EventHandler : IEventHandler
    {
        private readonly ITurboEventHub _eventHub;

        public EventHandler(ITurboEventHub eventHub)
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