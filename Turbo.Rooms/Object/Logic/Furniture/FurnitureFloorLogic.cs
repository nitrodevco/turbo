using System;
using System.Collections.Generic;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Attributes;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Arguments;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    [RoomObjectLogic("default_floor")]
    public class FurnitureFloorLogic : FurnitureLogicBase, IRollingObjectLogic, IFurnitureFloorLogic
    {
        public IRoomObjectFloor RoomObject { get; private set; }
        private IRollerData _rollerData;

        protected override void CleanUp()
        {
            _rollerData = null;

            base.CleanUp();
        }

        public bool SetRoomObject(IRoomObjectFloor roomObject)
        {
            if (roomObject == RoomObject) return true;

            if (RoomObject != null)
            {
                RoomObject.SetLogic(null);
            }

            if (roomObject == null)
            {
                Dispose();

                RoomObject = null;

                return false;
            }

            RoomObject = roomObject;

            RoomObject.SetLogic(this);

            return true;
        }

        public override void RefreshFurniture()
        {
            RoomObject.Room.SendComposer(new ObjectUpdateMessage
            {
                Object = RoomObject
            });
        }

        public override void RefreshStuffData()
        {
            RoomObject.Room.SendComposer(new ObjectDataUpdateMessage
            {
                Object = RoomObject
            });
        }

        public override bool SetState(int state, bool refresh = true)
        {
            if (StuffData == null) return false;

            if (state == StuffData.GetState()) return false;

            StuffData.SetState(state.ToString());

            if (RoomObject.RoomObjectHolder is IRoomFloorFurniture floorFurniture) floorFurniture.Save();

            if (refresh) RefreshStuffData();

            return true;
        }

        public virtual void OnEnter(IRoomObjectAvatar avatar)
        {
            RoomObject.Room.RoomWiredManager.ProcessTriggers<FurnitureWiredTriggerWalksOnFurniLogic>(new WiredArguments
            {
                UserObject = avatar,
                FurnitureObject = RoomObject
            });

            return;
        }

        public virtual void OnLeave(IRoomObjectAvatar avatar)
        {
            RoomObject.Room.RoomWiredManager.ProcessTriggers<FurnitureWiredTriggerWalksOffFurniLogic>(new WiredArguments
            {
                UserObject = avatar,
                FurnitureObject = RoomObject
            });

            return;
        }

        public virtual void BeforeStep(IRoomObjectAvatar roomObject)
        {
            return;
        }

        public virtual void OnStep(IRoomObjectAvatar roomObject)
        {
            return;
        }

        public virtual void OnStop(IRoomObjectAvatar avatar)
        {
            if (avatar.Logic is not AvatarLogic avatarLogic) return;

            if (CanSit())
            {
                avatarLogic.Sit(true, StackHeight, RoomObject.Location.Rotation);

                return;
            }

            if (CanLay())
            {
                avatarLogic.Lay(true, StackHeight, RoomObject.Location.Rotation);

                return;
            }
        }

        public override void OnInteract(IRoomObjectAvatar avatar, int param)
        {
            RoomObject.Room.RoomWiredManager.ProcessTriggers<FurnitureWiredTriggerStateChangeLogic>(new WiredArguments
            {
                UserObject = avatar,
                FurnitureObject = RoomObject
            });

            base.OnInteract(avatar, param);
        }

        public virtual bool CanStack() => FurnitureDefinition.CanStack;

        public virtual bool CanWalk(IRoomObjectAvatar avatar = null) => FurnitureDefinition.CanWalk;

        public virtual bool CanSit(IRoomObjectAvatar avatar = null) => FurnitureDefinition.CanSit;

        public virtual bool CanLay(IRoomObjectAvatar avatar = null) => FurnitureDefinition.CanLay;

        public virtual bool CanRoll() => true;

        public override bool CanToggle(IRoomObjectAvatar avatar)
        {
            if (UsagePolicy == FurniUsagePolicy.Nobody) return false;

            if (UsagePolicy == FurniUsagePolicy.Controller)
            {
                if (avatar.RoomObjectHolder is IRoomManipulator roomManipulator)
                {
                    if (RoomObject.Room.RoomSecurityManager.GetControllerLevel(roomManipulator) >= RoomControllerLevel.Rights) return true;
                }

                return false;
            }

            return true;
        }

        public virtual bool IsOpen(IRoomObjectAvatar avatar = null) => CanWalk(avatar) || CanSit(avatar) || CanLay(avatar);

        public IRoomTile GetCurrentTile() => RoomObject?.Room?.RoomMap?.GetTile(RoomObject.Location);

        public IList<IRoomTile> GetCurrentTiles()
        {
            var tiles = new List<IRoomTile>();

            if (RoomObject != null)
            {
                var points = AffectedPoints.GetPoints(RoomObject);

                foreach (var point in points)
                {
                    var tile = RoomObject.Room?.RoomMap?.GetTile(point);

                    if (tile == null) continue;

                    tiles.Add(tile);
                }
            }

            return tiles;
        }

        public virtual double StackHeight => FurnitureDefinition.Z;

        public double Height => RoomObject.Location.Z + StackHeight;

        public bool IsRolling => _rollerData != null;

        public IRollerData RollerData
        {
            get => _rollerData;
            set
            {
                if (_rollerData != null)
                {
                    _rollerData.RemoveRoomObject(RoomObject);
                }

                _rollerData = value;
            }
        }
    }
}
