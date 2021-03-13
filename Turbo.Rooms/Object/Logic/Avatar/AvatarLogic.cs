using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class AvatarLogic : MovingAvatarLogic
    {
        public int DanceType { get; private set; }

        public override void ProcessUpdateMessage(RoomObjectUpdateMessage updateMessage)
        {
            base.ProcessUpdateMessage(updateMessage);

            if (updateMessage is RoomObjectAvatarDanceMessage danceMessage)
            {
                HandleRoomObjectAvatarDanceMessage(danceMessage);

                return;
            }
        }

        private void HandleRoomObjectAvatarDanceMessage(RoomObjectAvatarDanceMessage danceMessage)
        {
            if (danceMessage == null) return;


        }

        public void Dance(RoomObjectAvatarDanceType danceType)
        {

        }

        public override void Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if(flag)
            {
                Dance(RoomObjectAvatarDanceType.NONE);

            }

            base.Sit(flag, height, rotation);
        }

        public override void Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                Dance(RoomObjectAvatarDanceType.NONE);

            }

            base.Sit(flag, height, rotation);
        }
    }
}
