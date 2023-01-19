using System.Text.Json;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;
using Turbo.Packets.Outgoing.Wired;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    public class FurnitureWiredConditionLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredConditionData();

            return JsonSerializer.Deserialize<WiredConditionData>(jsonString);
        }

        public override void SendConfigToSession(ISession session)
        {
            if (session == null) return;

            session.Send(new WiredConditionDataMessage
            {
                WiredData = WiredData
            });
        }

        public override IWiredConditionData WiredData => (IWiredConditionData)_wiredData;
    }
}
