using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class MovingAvatarLogic : RoomObjectLogicBase
    {
        public IDictionary<string, string> Statuses { get; private set; }

        public IPoint LocationNext { get; private set; }
        public IPoint LocationGoal { get; private set; }
        public IList<IPoint> CurrentPath { get; private set; }
        
        public bool IsWalking { get; private set; }
        public bool CanWalk { get; private set; }

        public void StopWalking()
        {
            if (!IsWalking) return;

            ClearWalking();
        }

        private void ClearWalking()
        {
            ClearPath();
        }

        private void ClearPath()
        {
            if (CurrentPath == null) return;

            CurrentPath.Clear();
        }

        public void ProcessNextLocation()
        {
            if (LocationNext == null) return;

            RoomObject.SetLocation(LocationNext);

            LocationNext = null;

            // get the tile
            // if tile is door, dispose this room object
            // if we are on a tile update our height
        }

        public void UpdateHeight(IRoomTile roomTile = null)
        {

        }
    }


}
