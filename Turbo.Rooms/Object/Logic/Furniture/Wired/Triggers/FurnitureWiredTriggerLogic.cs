using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;
using Turbo.Packets.Outgoing.Wired;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    public class FurnitureWiredTriggerLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredTriggerData();

            return JsonSerializer.Deserialize<WiredTriggerData>(jsonString);
        }

        public override void SendConfigToSession(ISession session)
        {
            if (session == null) return;

            session.Send(new WiredTriggerDataMessage
            {
                WiredData = WiredData
            });
        }

        public override IWiredTriggerData WiredData => (IWiredTriggerData)_wiredData;
    }
}
