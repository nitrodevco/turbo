using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;
using System;
using Turbo.Core.Game.Rooms.Object.Data;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired
{
    public class FurnitureWiredLogic : FurnitureLogic, IFurnitureWiredLogic
    {
        private static readonly int _offState = 0;
        private static readonly int _onState = 1;

        protected IWiredData _wiredData;

        public override async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (!await base.Setup(furnitureDefinition, jsonString)) return false;

            //SetState(_offState);

            return true;
        }

        public void SetupWiredData(string jsonString = null)
        {
            IWiredData wiredData = CreateWiredDataFromJson(jsonString);

            if(ValidateWiredData(wiredData)) _wiredData = wiredData;
        }

        public virtual IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredDataBase();

            return JsonSerializer.Deserialize<WiredDataBase>(jsonString);
        }

        protected virtual bool ValidateWiredData(IWiredData wiredData)
        {
            if (wiredData.SelectionIds.Count > 0)
            {
                List<int> furnitureIds = new();

                foreach (int id in wiredData.SelectionIds)
                {
                    if (furnitureIds.Count == wiredData.SelectionLimit) break;

                    IFurniture furniture = RoomObject.Room.RoomFurnitureManager.GetFurniture(id);

                    if (furniture == null) continue;

                    furnitureIds.Add(id);
                }

                ((List<int>)wiredData.SelectionIds).Sort();
                furnitureIds.Sort();

                string previous = string.Join(",", wiredData.SelectionIds);
                string valid = string.Join(",", furnitureIds);

                if (!previous.Equals(valid))
                {
                    wiredData.SelectionIds = furnitureIds;

                    if(RoomObject.RoomObjectHolder is IFurniture furniture)
                    {
                        furniture.Save();
                    }
                }
            }

            return true;
        }

        public override async Task Cycle()
        {

        }

        public override void OnInteract(IRoomObject roomObject, int param)
        {
            if (!CanToggle(RoomObject)) return;

            // send wired config (class specific)
        }

        public virtual bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (_wiredData == null) return false;

            return true;
        }

        public virtual void OnTriggered(IWiredArguments wiredArguments = null)
        {
            ProcessAnimation();
        }

        protected virtual void ProcessAnimation()
        {
            SetState(_onState);
            SetState(_offState, false);
        }

        public virtual IWiredData WiredData => _wiredData;
        public int WiredKey => 0;
    }
}
