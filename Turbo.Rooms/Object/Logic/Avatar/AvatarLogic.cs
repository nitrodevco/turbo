using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Packets.Outgoing.Room.Action;

namespace Turbo.Rooms.Object.Logic.Avatar;

public class AvatarLogic : MovingAvatarLogic
{
    private int _remainingHeadCycles;
    public RoomObjectAvatarDanceType DanceType { get; private set; }

    public override bool OnReady()
    {
        if (!base.OnReady()) return false;

        _remainingHeadCycles = -1;

        return true;
    }

    public override async Task Cycle()
    {
        await base.Cycle();

        if (_remainingHeadCycles > -1)
        {
            if (_remainingHeadCycles == 0)
            {
                RoomObject.HeadRotation = RoomObject.Location.Rotation;

                RoomObject.NeedsUpdate = true;

                _remainingHeadCycles = -1;

                return;
            }

            _remainingHeadCycles--;
        }
    }

    public override void InvokeCurrentLocation()
    {
        var roomTile = GetCurrentTile();

        if (roomTile == null) return;

        if (!roomTile.CanSit() || !roomTile.CanLay())
        {
            Sit(false);
            Lay(false);
        }

        base.InvokeCurrentLocation();
    }

    public virtual bool Sit(bool flag = true, double height = 0.50, Rotation? rotation = null)
    {
        if (flag)
        {
            if (HasStatus(RoomObjectAvatarStatus.Sit)) return false;

            Dance(RoomObjectAvatarDanceType.None);
            RemoveStatus(RoomObjectAvatarStatus.Lay);

            rotation = rotation == null ? RoomObject.Location.CalculateSitRotation() : rotation;

            RoomObject.Rotation = (Rotation)rotation;
            RoomObject.HeadRotation = (Rotation)rotation;

            AddStatus(RoomObjectAvatarStatus.Sit, string.Format("{0:N3}", height));
        }
        else
        {
            if (!HasStatus(RoomObjectAvatarStatus.Sit)) return false;

            RemoveStatus(RoomObjectAvatarStatus.Sit);
        }

        return true;
    }

    public virtual bool Lay(bool flag = true, double height = 0.50, Rotation? rotation = null)
    {
        if (flag)
        {
            if (HasStatus(RoomObjectAvatarStatus.Lay)) return false;

            Dance(RoomObjectAvatarDanceType.None);
            RemoveStatus(RoomObjectAvatarStatus.Sit);

            rotation = rotation == null ? RoomObject.Location.CalculateSitRotation() : rotation;

            RoomObject.Rotation = (Rotation)rotation;
            RoomObject.HeadRotation = (Rotation)rotation;

            AddStatus(RoomObjectAvatarStatus.Lay, string.Format("{0:N3}", height));
        }
        else
        {
            if (!HasStatus(RoomObjectAvatarStatus.Lay)) return false;

            RemoveStatus(RoomObjectAvatarStatus.Lay);
        }

        return true;
    }

    public virtual bool LookAtPoint(IPoint point, bool headOnly = false, bool selfInvoked = true)
    {
        if (RoomObject.Location.Compare(point) || HasStatus(RoomObjectAvatarStatus.Lay)) return false;

        if (headOnly || HasStatus(RoomObjectAvatarStatus.Sit))
        {
            RoomObject.HeadRotation = RoomObject.Location.CalculateHeadRotation(point);

            _remainingHeadCycles = DefaultSettings.HeadTurnCycles;
        }
        else
        {
            RoomObject.Rotation = RoomObject.Location.CalculateHumanRotation(point);
            RoomObject.HeadRotation = RoomObject.Rotation;
        }

        RoomObject.NeedsUpdate = true;

        return true;
    }

    public virtual bool Dance(RoomObjectAvatarDanceType danceType)
    {
        if (danceType == DanceType) return false;

        if (HasStatus(RoomObjectAvatarStatus.Sit, RoomObjectAvatarStatus.Lay)) return false;

        // check if the dance type is valid
        // check if the dance is hc only and validate subscription status

        DanceType = danceType;

        RoomObject.Room.SendComposer(new DanceMessage
        {
            ObjectId = RoomObject.Id,
            DanceStyle = (int)danceType
        });

        return true;
    }

    public virtual bool Expression(RoomObjectAvatarExpression expressionType)
    {
        if (expressionType == RoomObjectAvatarExpression.Idle) return false;

        // check if the expression type is valid
        // check if the expression is hc only and validate subscription status

        RoomObject.Room.SendComposer(new ExpressionMessage
        {
            ObjectId = RoomObject.Id,
            ExpressionType = (int)expressionType
        });

        return true;
    }

    public virtual bool Sign(int sign)
    {
        if (sign < 0 || sign > 17) return false;

        if (HasStatus(RoomObjectAvatarStatus.Lay)) return false;

        AddStatus(RoomObjectAvatarStatus.Sign, sign.ToString());

        return true;
    }
}