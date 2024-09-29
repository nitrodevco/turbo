using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;
using Turbo.Packets.Outgoing.Navigator;

namespace Turbo.Main.PacketHandlers;

public sealed class NavigatorMessageHandler(
    IPacketMessageHub messageHub,
    INavigatorManager navigatorManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<CreateFlatMessage>(this, OnCreateFlatMessage);
        messageHub.Subscribe<GetUserFlatCatsMessage>(this, OnGetUserFlatCatsMessage);
        messageHub.Subscribe<GetGuestRoomMessage>(this, OnGetGuestRoomMessage);
        messageHub.Subscribe<NewNavigatorInitMessage>(this, OnNewNavigatorInitMessage);
        messageHub.Subscribe<NewNavigatorSearchMessage>(this, OnNewNavigatorSearchMessage);
        messageHub.Subscribe<AddFavouriteRoomMessage>(this, OnAddFavouriteRoomMessage);
        messageHub.Subscribe<DeleteFavouriteRoomMessage>(this, OnDeleteFavouriteRoomMessage);
    }

    private async Task OnCreateFlatMessage(CreateFlatMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.CreateFlat(session.Player, message.FlatName, message.FlatDescription, message.FlatModelName, message.MaxPlayers, message.CategoryID, message.TradeSetting);
    }

    private async Task OnGetUserFlatCatsMessage(GetUserFlatCatsMessage message, ISession session)
    {
        if (session.Player == null) return;
        await navigatorManager.SendNavigatorCategories(session.Player);
    }

    private async Task OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
        await navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom, message.RoomForward);
    }

    private async Task OnNewNavigatorInitMessage(NewNavigatorInitMessage message, ISession session)
    {
        if (session.Player == null) return;
        await navigatorManager.SendNavigatorSettings(session.Player);
        await navigatorManager.SendNavigatorMetaData(session.Player);
        await navigatorManager.SendNavigatorLiftedRooms(session.Player);
        await navigatorManager.SendNavigatorCollapsedCategories(session.Player);
        await navigatorManager.SendNavigatorSavedSearches(session.Player);
        await navigatorManager.SendNavigatorEventCategories(session.Player);
    }

    private async Task OnNewNavigatorSearchMessage(NewNavigatorSearchMessage message, ISession session)
    {
        if (session.Player == null) return;

        var searchCode = message.SearchCodeOriginal?.ToLower() ?? string.Empty;
        var searchTerm = message.FilteringData ?? string.Empty;
        var filterMode = "anything";

        if (!string.IsNullOrEmpty(searchTerm) && searchTerm.Contains(':'))
        {
            var parts = searchTerm.Split(new[] { ':' }, 2);

            filterMode = parts[0].Trim() switch
            {
                "tag" => "tag",
                "owner" => "owner",
                "roomname" => "roomname",
                "group" => "group",
                _ => "anything"
            };

            if (!filterMode.Equals("anything"))
            {
                searchTerm = parts[1].Trim();
            }
        }

        await navigatorManager.HandleNavigatorSearch(session.Player, searchCode, searchTerm, filterMode);
    }

    private async Task OnAddFavouriteRoomMessage(AddFavouriteRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
        var playerId = session.Player.Id;
        var roomId = message.RoomId;

        await navigatorManager.HandleFavouriteRoomChangeAsync(playerId, roomId, true);
        await SendFavouriteChangedMessage(session, roomId, true);
    }

    private async Task OnDeleteFavouriteRoomMessage(DeleteFavouriteRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
        var playerId = session.Player.Id;
        var roomId = message.RoomId;

        await navigatorManager.HandleFavouriteRoomChangeAsync(playerId, roomId, false);
        await SendFavouriteChangedMessage(session, roomId, false);
    }

    private async Task SendFavouriteChangedMessage(ISession session, int roomId, bool isAdded) => await session.Send(new FavouriteChangedMessage(roomId, isAdded));
}