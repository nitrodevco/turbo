using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Avatar;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Main.PacketHandlers;

public class RoomAvatarMessageHandler : IRoomAvatarMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly IRoomManager _roomManager;

    public RoomAvatarMessageHandler(
        IPacketMessageHub messageHub,
        IRoomManager roomManager)
    {
        _messageHub = messageHub;
        _roomManager = roomManager;

        _messageHub.Subscribe<AvatarExpressionMessage>(this, OnAvatarExpressionMessage);
        _messageHub.Subscribe<ChangeMottoMessage>(this, OnChangeMottoMessage);
        _messageHub.Subscribe<ChangePostureMessage>(this, OnChangePostureMessage);
        _messageHub.Subscribe<DanceMessage>(this, OnDanceMessage);
        _messageHub.Subscribe<DropCarryItemMessage>(this, OnDropCarryItemMessage);
        _messageHub.Subscribe<LookToMessage>(this, OnLookToMessage);
        _messageHub.Subscribe<PassCarryItemMessage>(this, OnPassCarryItemMessage);
        _messageHub.Subscribe<PassCarryItemToPetMessage>(this, OnPassCarryItemToPetMessage);
        _messageHub.Subscribe<SignMessage>(this, OnSignMessage);
    }

    private void OnAvatarExpressionMessage(AvatarExpressionMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic)
            avatarLogic.Expression((RoomObjectAvatarExpression)message.TypeCode);
    }

    private void OnChangeMottoMessage(ChangeMottoMessage message, ISession session)
    {
        if (session.Player == null) return;

        IRoomObject roomObject = session.Player.RoomObject;

        if (roomObject == null) return;
    }

    private void OnChangePostureMessage(ChangePostureMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic)
            switch ((RoomObjectAvatarPosture)message.Posture)
            {
                case RoomObjectAvatarPosture.Sit:
                    avatarLogic.Sit();
                    return;
            }
    }

    private void OnDanceMessage(DanceMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic)
            avatarLogic.Dance((RoomObjectAvatarDanceType)message.Style);
    }

    private void OnDropCarryItemMessage(DropCarryItemMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic)
        {
        }
    }

    private void OnLookToMessage(LookToMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic)
            avatarLogic.LookAtPoint(new Point(message.LocX, message.LocY));
    }

    private void OnPassCarryItemMessage(PassCarryItemMessage message, ISession session)
    {
    }

    private void OnPassCarryItemToPetMessage(PassCarryItemToPetMessage message, ISession session)
    {
    }

    private void OnSignMessage(SignMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (session.Player.RoomObject?.Logic is AvatarLogic avatarLogic) avatarLogic.Sign(message.SignId);
    }
}