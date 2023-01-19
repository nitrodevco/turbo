using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Packets.Incoming.Wired;
using Turbo.Core.Packets.Messages;
using System.Text.Json.Serialization;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types
{
    public class WiredActionData : WiredDataBase, IWiredActionData
    {
        public int Delay { get; protected set; }
        [JsonIgnore]
        public IList<int> Conflicts { get; protected set; }

        public WiredActionData() : base()
        {
            Delay = 0;
            Conflicts = new List<int>();
        }

        public override bool SetFromMessage(IMessageEvent update)
        {
            if (!base.SetFromMessage(update)) return false;

            if (update is UpdateActionMessage actionMessage)
            {
                Delay = actionMessage.Delay;

                return true;
            }

            return false;
        }
    }
}
