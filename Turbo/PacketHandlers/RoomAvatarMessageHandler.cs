using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Avatar;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Main.PacketHandlers;

public class RoomAvatarMessageHandler(
    IPacketMessageHub messageHub) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<AvatarExpressionMessage>(this, OnAvatarExpressionMessage);
        messageHub.Subscribe<ChangeMottoMessage>(this, OnChangeMottoMessage);
        messageHub.Subscribe<ChangePostureMessage>(this, OnChangePostureMessage);
        messageHub.Subscribe<DanceMessage>(this, OnDanceMessage);
        messageHub.Subscribe<DropCarryItemMessage>(this, OnDropCarryItemMessage);
        messageHub.Subscribe<LookToMessage>(this, OnLookToMessage);
        messageHub.Subscribe<PassCarryItemMessage>(this, OnPassCarryItemMessage);
        messageHub.Subscribe<PassCarryItemToPetMessage>(this, OnPassCarryItemToPetMessage);
        messageHub.Subscribe<SignMessage>(this, OnSignMessage);
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