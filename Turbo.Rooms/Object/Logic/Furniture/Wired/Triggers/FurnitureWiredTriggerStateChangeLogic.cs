using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    [RoomObjectLogic("wf_trg_state_changed")]
    public class FurnitureWiredTriggerStateChangeLogic : FurnitureWiredTriggerLogic
    {
        public override bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (!base.CanTrigger(wiredArguments)) return false;

            if (wiredArguments.FurnitureObject == null) return false;

            if (!WiredData.SelectionIds.Contains(wiredArguments.FurnitureObject.Id)) return false;

            return true;
        }

        public override int WiredKey => (int)FurnitureWiredTriggerType.ToggleFurni;
    }
}
