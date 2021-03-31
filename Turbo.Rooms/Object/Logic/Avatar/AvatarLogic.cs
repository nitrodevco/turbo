using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Action;

namespace Turbo.Rooms.Object.Logic.Avatar
{
    public class AvatarLogic : MovingAvatarLogic
    {
        private static readonly int _idleCycles = 1200;
        private static readonly int _headTurnCycles = 6;

        public RoomObjectAvatarDanceType DanceType { get; private set; }
        public bool IsIdle { get; private set; }

        private int _remainingIdleCycles;
        private int _remainingHeadCycles;

        public override bool OnReady()
        {
            if (!base.OnReady()) return false;

            _remainingIdleCycles = _idleCycles;
            _remainingHeadCycles = -1;

            return true;
        }

        public override async Task Cycle()
        {
            await base.Cycle();

            if(_remainingIdleCycles > -1)
            {
                if (_remainingIdleCycles == 0)
                {
                    Idle(true);

                    _remainingIdleCycles = -1;

                    return;
                }

                _remainingIdleCycles--;
            }

            if (_remainingHeadCycles > -1)
            {
                if (_remainingHeadCycles == 0)
                {
                    RoomObject.Location.HeadRotation = RoomObject.Location.Rotation;

                    RoomObject.NeedsUpdate = true;

                    _remainingHeadCycles = -1;

                    return;
                }

                _remainingHeadCycles--;
            }
        }

        public override void WalkTo(IPoint location, bool selfWalk = false)
        {
            base.WalkTo(location, selfWalk);

            Idle(false);
        }

        public override void Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                Dance(RoomObjectAvatarDanceType.None);
            }

            Idle(false);

            base.Sit(flag, height, rotation);
        }

        public override void Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
        {
            if (flag)
            {
                Dance(RoomObjectAvatarDanceType.None);
            }

            Idle(false);

            base.Lay(flag, height, rotation);
        }

        public void LookAtPoint(IPoint point, bool headOnly = false, bool selfInvoked = true)
        {
            if (selfInvoked) Idle(false);

            if (IsWalking || IsIdle) return;

            if (RoomObject.Location.Compare(point)) return;

            if (HasStatus(RoomObjectAvatarStatus.Lay)) return;

            if(headOnly || HasStatus(RoomObjectAvatarStatus.Sit))
            {
                RoomObject.Location.HeadRotation = RoomObject.Location.CalculateHeadRotation(point);

                _remainingHeadCycles = _headTurnCycles;
            }
            else
            {
                RoomObject.Location.SetRotation(RoomObject.Location.CalculateHumanRotation(point));
            }

            RoomObject.NeedsUpdate = true;
        }

        public void Dance(RoomObjectAvatarDanceType danceType)
        {
            if (danceType == DanceType) return;

            if (HasStatus(RoomObjectAvatarStatus.Sit, RoomObjectAvatarStatus.Lay)) return;

            // check if the dance type is valid
            // check if the dance is hc only and validate subscription status

            Idle(false);

            DanceType = danceType;

            RoomObject.Room.SendComposer(new DanceMessage
            {
                UserId = RoomObject.Id,
                DanceStyle = (int)danceType
            });
        }

        public void Expression(RoomObjectAvatarExpression expressionType)
        {
            if(expressionType == RoomObjectAvatarExpression.Idle)
            {
                Idle(true);

                return;
            }

            // check if the expression type is valid
            // check if the expression is hc only and validate subscription status

            Idle(false);

            RoomObject.Room.SendComposer(new ExpressionMessage
            {
                UserId = RoomObject.Id,
                ExpressionType = (int)expressionType
            });
        }

        public void Sign(int sign)
        {
            if (sign < 0 || sign > 17) return;

            if (HasStatus(RoomObjectAvatarStatus.Lay)) return;

            Idle(false);

            AddStatus(RoomObjectAvatarStatus.Sign, sign.ToString());
        }

        public void Idle(bool flag)
        {
            if(!flag)
            {
                ResetIdleCycle();
            }
            else
            {
                _remainingIdleCycles = -1;
            }

            if (flag == IsIdle) return;

            IsIdle = flag;

            RoomObject.Room.SendComposer(new SleepMessage
            {
                UserId = RoomObject.Id,
                Sleeping = IsIdle
            });
        }

        private void ResetIdleCycle()
        {
            _remainingIdleCycles = _idleCycles;
        }
    }
}
