using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Action;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Avatar;

[RoomObjectLogic("user")]
public class PlayerLogic : AvatarLogic
{
    private int _remainingIdleCycles;
    public bool IsIdle { get; private set; }
    public bool IsKicked { get; private set; }

    public override bool OnReady()
    {
        if (!base.OnReady()) return false;

        _remainingIdleCycles = DefaultSettings.AvatarIdleCycles;

        return true;
    }

    public override async Task Cycle()
    {
        await base.Cycle();

        if (_remainingIdleCycles > -1)
        {
            if (_remainingIdleCycles == 0)
            {
                Idle(true);

                _remainingIdleCycles = -1;

                return;
            }

            _remainingIdleCycles--;
        }
    }

    public override void StopWalking()
    {
        var roomTile = GetCurrentTile();

        if (IsKicked || (roomTile != null && roomTile.IsDoor && DidMove))
        {
            RoomObject.Dispose();

            return;
        }

        base.StopWalking();
    }

    public virtual void Kick()
    {
        IsKicked = true;

        var doorLocation = RoomObject.Room.RoomModel.DoorLocation;

        if (RoomObject.Location.Compare(doorLocation))
        {
            RoomObject.Dispose();

            return;
        }

        WalkTo(doorLocation);
    }

    public virtual void Idle(bool flag)
    {
        if (!flag)
            ResetIdleCycle();
        else
            _remainingIdleCycles = -1;

        if (flag == IsIdle) return;

        IsIdle = flag;

        RoomObject.Room.SendComposer(new SleepMessage
        {
            ObjectId = RoomObject.Id,
            Sleeping = IsIdle
        });
    }

    public override void WalkTo(IPoint location, bool selfWalk = false)
    {
        if (IsKicked && selfWalk) return;

        base.WalkTo(location, selfWalk);

        Idle(false);
    }

    public override void GoTo(IPoint location, bool selfWalk = false)
    {
        base.GoTo(location, selfWalk);

        Idle(false);
    }

    public override bool Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
    {
        if (!base.Sit(flag, height, rotation)) return false;

        Idle(false);

        return true;
    }

    public override bool Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
    {
        if (!base.Lay(flag, height, rotation)) return false;

        Idle(false);

        return true;
    }

    public override bool LookAtPoint(IPoint point, bool headOnly = false, bool selfInvoked = true)
    {
        if (IsWalking || IsIdle) return false;

        if (!base.LookAtPoint(point, headOnly, selfInvoked)) return false;

        if (selfInvoked) Idle(false);

        return true;
    }

    public override bool Dance(RoomObjectAvatarDanceType danceType)
    {
        if (!base.Dance(danceType)) return false;

        Idle(false);

        return true;
    }

    public override bool Expression(RoomObjectAvatarExpression expressionType)
    {
        if (expressionType == RoomObjectAvatarExpression.Idle)
        {
            Idle(true);

            return false;
        }

        if (!base.Expression(expressionType)) return false;

        Idle(false);

        return true;
    }

    public override bool Sign(int sign)
    {
        if (!base.Sign(sign)) return false;

        Idle(false);

        return true;
    }

    private void ResetIdleCycle()
    {
        _remainingIdleCycles = DefaultSettings.AvatarIdleCycles;
    }
}