using System;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("dice")]
public class FurnitureDiceLogic : FurnitureFloorLogic
{
    private static readonly int[] _diceNumbers = { 1, 2, 3, 4, 5, 6 };
    private static readonly int _rollingState = -1;
    private static readonly int _closedState = 0;

    private int _remainingDiceCycles = -1;

    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Everybody;

    public override async Task Cycle()
    {
        if (_remainingDiceCycles > -1)
        {
            if (_remainingDiceCycles > 0) _remainingDiceCycles--;

            if (_remainingDiceCycles != 0) return;

            _remainingDiceCycles = -1;

            SetState(_diceNumbers[new Random().Next(0, _diceNumbers.Length)]);
        }
    }

    public virtual void ThrowDice(IRoomObjectAvatar avatar)
    {
        if (!CanToggle(avatar)) return;

        SetState(_rollingState);

        _remainingDiceCycles = DefaultSettings.DiceCycles;
    }

    public virtual void DiceOff(IRoomObjectAvatar avatar)
    {
        if (!CanToggle(avatar)) return;

        SetState(_closedState);

        _remainingDiceCycles = -1;
    }

    public override void OnInteract(IRoomObjectAvatar avatar, int param)
    {
    }

    public override bool CanToggle(IRoomObjectAvatar avatar)
    {
        if (RoomObject.Location.GetDistanceAround(avatar.Location) > 2) return false;

        return base.CanToggle(avatar);
    }
}