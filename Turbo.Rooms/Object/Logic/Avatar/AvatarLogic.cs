using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class AvatarLogic : MovingAvatarLogic
    {
        public RoomObjectAvatarDanceType DanceType { get; private set; }
        public bool IsIdle { get; private set; }

        public override void WalkTo(IPoint location)
        {
            base.WalkTo(location);

            Idle(false);
        }

        public override void Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                Dance(RoomObjectAvatarDanceType.None);
            }

            base.Sit(flag, height, rotation);
        }

        public override void Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                Dance(RoomObjectAvatarDanceType.None);
            }

            base.Lay(flag, height, rotation);
        }

        public void Dance(RoomObjectAvatarDanceType danceType)
        {
            if (danceType == DanceType) return;

            if (HasStatus(RoomObjectAvatarStatus.Sit, RoomObjectAvatarStatus.Lay)) return;

            // check if the dance type is valid
            // check if the dance is hc only and validate subscription status

            DanceType = danceType;

            // COMPOSER: ROOM USER DANCE (RoomObject.Id, DanceType)
        }

        public void Idle(bool flag)
        {
            // if false, start the timer
            // else stop the timer

            if (flag == IsIdle) return;

            IsIdle = flag;

            // COMPOSER: ROOM USER IDLE (RoomObject.Id, IsIdle)
        }
    }
}
