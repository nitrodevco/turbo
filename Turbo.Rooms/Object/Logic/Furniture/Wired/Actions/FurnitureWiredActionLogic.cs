using System.Text.Json;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;
using Turbo.Packets.Outgoing.Wired;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Actions
{
    public class FurnitureWiredActionLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredActionData();

            return JsonSerializer.Deserialize<WiredActionData>(jsonString);
        }

        public override void SendConfigToSession(ISession session)
        {
            if (session == null) return;

            session.Send(new WiredEffectDataMessage
            {
                WiredData = WiredData
            });
        }

        public override IWiredActionData WiredData => (IWiredActionData)_wiredData;
    }
}
