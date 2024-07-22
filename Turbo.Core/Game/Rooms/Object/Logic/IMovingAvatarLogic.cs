using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IMovingAvatarLogic : IRoomObjectLogic, IRollingObjectLogic
{
    public IRoomObjectAvatar RoomObject { get; }

    public IDictionary<string, string> Statuses { get; }
    public IPoint LocationNext { get; set; }
    public IPoint LocationPrevious { get; }
    public IPoint LocationGoal { get; }
    public IList<IPoint> CurrentPath { get; }
    public Action<IRoomObjectAvatar> BeforeGoalAction { get; set; }
    public Action<IRoomObjectAvatar> GoalAction { get; set; }
    public bool NeedsRepathing { get; set; }
    public bool IsWalking { get; }
    public bool CanWalk { get; set; }
    public bool DidMove { get; }
    public bool SetRoomObject(IRoomObjectAvatar roomObject);
    public void WalkTo(IPoint location, bool selfWalk = false);
    public void GoTo(IPoint location, bool selfWalk = false);
    public void ResetPath();
    public void StopWalking();
    public void ClearPath();
    public bool ProcessNextLocation();
    public void UpdateHeight(IRoomTile roomTile = null);
    public void InvokeCurrentLocation();
    public void InvokeBeforeGoalAction();
    public void InvokeGoalAction();
    public void AddStatus(string type, string value);
    public bool HasStatus(params string[] types);
    public void RemoveStatus(params string[] types);
    public IRoomTile GetCurrentTile();
    public IRoomTile GetNextTile();
}