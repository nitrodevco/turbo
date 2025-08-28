using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Furniture;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Furniture;
using Turbo.Packets.Outgoing.Room.Furniture;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Main.PacketHandlers;

public class RoomFurnitureMessageHandler(IPacketMessageHub messageHub) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<DiceOffMessage>(this, OnDiceOffMessage);
        messageHub.Subscribe<RoomDimmerChangeStateMessage>(this, OnRoomDimmerChangeState);
        messageHub.Subscribe<RoomDimmerGetPresetsMessage>(this, OnRoomDimmerGetPresets);
        messageHub.Subscribe<RoomDimmerSavePresetMessage>(this, OnRoomDimmerSavePreset);
        messageHub.Subscribe<SetCustomStackingHeightMessage>(this, OnSetCustomStackingHeightMessage);
        messageHub.Subscribe<ThrowDiceMessage>(this, OnThrowDiceMessage);
    }

    protected virtual void OnDiceOffMessage(DiceOffMessage message, ISession session)
    {
        if (session.Player is null) return;

        var diceObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.ObjectId);

        if (diceObject is null) return;

        if (diceObject.Logic is FurnitureDiceLogic diceLogic) diceLogic.DiceOff(session.Player.RoomObject);
    }

    protected virtual void OnRoomDimmerChangeState(RoomDimmerChangeStateMessage message, ISession session)
    {
        if (session.Player is null) return;

        var moodLightObject = session.Player.RoomObject?.Room.RoomFurnitureManager.WallObjects.GetRoomObject(message.ObjectId);

        if (moodLightObject is null) return;

        if (moodLightObject.Logic is FurnitureMoodLightLogic dimmerLogic)
        {
            //Toggle the State, if is Enabled then turn to 1 (off), if is not enabled turn to 2 (On)
            dimmerLogic.SetState(dimmerLogic.IsEnabled ? 1 : 2);
        }
    }

    protected virtual void OnRoomDimmerGetPresets(RoomDimmerGetPresetsMessage message, ISession session)
    {
        if (session.Player is null) return;

        var moodLightObject = session.Player.RoomObject?.Room.RoomFurnitureManager.WallObjects.GetRoomObject(message.ObjectId);

        if (moodLightObject is null) return;

        if (moodLightObject.Logic is FurnitureMoodLightLogic dimmerLogic)
        {
            session.Send(new RoomDimmerPresetsMessage
            {
                SelectedPresetId = dimmerLogic.CurrentPreset.PresetId,
                Presets = dimmerLogic.Presets,
                IsOn = dimmerLogic.IsEnabled,
                ItemId = moodLightObject.Id
            });
        }
    }

    protected async Task OnRoomDimmerSavePreset(RoomDimmerSavePresetMessage message, ISession session)
    {
        if (session.Player is null) return;

        var moodLightObject = session.Player.RoomObject?.Room.RoomFurnitureManager.WallObjects.GetRoomObject(message.ObjectId);
        
        if (moodLightObject is null) return;

        if (moodLightObject.Logic is FurnitureMoodLightLogic dimmerLogic)
        {
            await dimmerLogic.SavePresetAsync(message.PresetId, message.EffectType, message.ColorHex, message.Brightness, message.Apply);
        }
    }

    protected virtual void OnSetCustomStackingHeightMessage(SetCustomStackingHeightMessage message, ISession session)
    {
        if (session.Player is null) return;

        var stackHelperObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.FurniId);

        if (stackHelperObject is null) return;

        if (stackHelperObject.Logic is FurnitureStackHelperLogic stackHelperLogic)
            stackHelperLogic.SetStackHelperHeight(session.Player.RoomObject, message.Height);
    }

    protected virtual void OnThrowDiceMessage(ThrowDiceMessage message, ISession session)
    {
        if (session.Player is null) return;

        var diceObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.ObjectId);

        if (diceObject is null) return;

        if (diceObject.Logic is FurnitureDiceLogic diceLogic) diceLogic.ThrowDice(session.Player.RoomObject);
    }
}