using System;
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
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Utilities;
using Turbo.Packets.Outgoing.Wired;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Game;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired
{
    public abstract class FurnitureWiredLogic : FurnitureFloorLogic, IFurnitureWiredLogic
    {
        private static readonly int _offState = 0;
        private static readonly int _onState = 1;

        protected IWiredData _wiredData;

        private long _lastRun;
        private Dictionary<int, long> _lastRunPlayers;
        private bool _needsOffState;

        public override async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (!await base.Setup(furnitureDefinition, jsonString)) return false;

            _lastRunPlayers = new();

            SetState(_offState);

            return true;
        }

        public void SetupWiredData(string jsonString = null)
        {
            var wiredData = CreateWiredDataFromJson(jsonString);

            ValidateWiredData(wiredData);

            wiredData.SetRoomObject(RoomObject);

            _wiredData = wiredData;
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

                    var furniture = RoomObject.Room.RoomFurnitureManager.GetFloorFurniture(id);

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

                    if (RoomObject.RoomObjectHolder is IRoomFloorFurniture furniture)
                    {
                        furniture.Save();
                    }
                }
            }

            return true;
        }

        public virtual bool SaveWiredData(IRoomObjectAvatar avatar, IWiredData wiredData)
        {
            if (!CanToggle(avatar)) return false;

            ValidateWiredData(wiredData);

            wiredData.SetRoomObject(RoomObject);

            _wiredData = wiredData;

            if (RoomObject.RoomObjectHolder is IRoomFloorFurniture floorFurniture) floorFurniture.Save();

            return true;
        }

        public override async Task Cycle()
        {
            if (_needsOffState)
            {
                SetState(_offState);

                _needsOffState = false;
            }
        }

        public override void OnInteract(IRoomObjectAvatar avatar, int param)
        {
            if (!CanToggle(avatar)) return;

            if (avatar.RoomObjectHolder is not IPlayer player) return;

            SendConfigToSession(player.Session);
        }

        public virtual bool CanTrigger(IWiredArguments wiredArguments = null)
        {
            if (_wiredData == null) return false;

            long lastRun = TimeUtilities.GetCurrentMilliseconds();

            if ((lastRun - _lastRun) < Cooldown) return false;

            if (wiredArguments.UserObject != null)
            {
                if (_lastRunPlayers.ContainsKey(wiredArguments.UserObject.Id))
                {
                    if ((lastRun - _lastRunPlayers[wiredArguments.UserObject.Id]) < CooldownPlayer) return false;

                    _lastRunPlayers[wiredArguments.UserObject.Id] = lastRun;
                }
                else
                {
                    _lastRunPlayers.Add(wiredArguments.UserObject.Id, lastRun);
                }
            }

            _lastRun = lastRun;

            return true;
        }

        public virtual void OnTriggered(IWiredArguments wiredArguments = null)
        {
            ProcessAnimation();
        }

        public abstract void SendConfigToSession(ISession session);

        protected virtual void ProcessAnimation()
        {
            SetState(_onState);

            _needsOffState = true;
        }

        public virtual IWiredData WiredData => _wiredData;
        public virtual int WiredKey => 0;

        public virtual int Cooldown => 50;
        public virtual int CooldownPlayer => 350;

        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Controller;
    }
}
