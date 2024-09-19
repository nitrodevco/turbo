using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Navigator.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;

namespace Turbo.Main.PacketHandlers;

public class NavigatorMessageHandler(
    IPacketMessageHub messageHub,
    INavigatorManager navigatorManager,
    ILogger<NavigatorMessageHandler> logger)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetUserFlatCatsMessage>(this, OnGetUserFlatCatsMessage);
        messageHub.Subscribe<GetGuestRoomMessage>(this, OnGetGuestRoomMessage);
        messageHub.Subscribe<NewNavigatorInitMessage>(this, OnNewNavigatorInitMessage);
        messageHub.Subscribe<NewNavigatorSearchMessage>(this, OnNewNavigatorSearchMessage);
    }

    protected virtual async void OnGetUserFlatCatsMessage(GetUserFlatCatsMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.SendNavigatorCategories(session.Player);
    }

    protected virtual async void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom,
            message.RoomForward);
    }

    protected virtual async void OnNewNavigatorInitMessage(NewNavigatorInitMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.SendNavigatorSettings(session.Player);
        await navigatorManager.SendNavigatorMetaData(session.Player);
        await navigatorManager.SendNavigatorLiftedRooms(session.Player);
        await navigatorManager.SendNavigatorCollapsedCategories(session.Player);
        await navigatorManager.SendNavigatorSavedSearches(session.Player);
        await navigatorManager.SendNavigatorEventCategories(session.Player);
    }
    
    protected virtual async void OnNewNavigatorSearchMessage(NewNavigatorSearchMessage message, ISession session)
    {
        if (session.Player == null) return;

        // Log the incoming message for debugging
        logger.LogInformation($"Received NewNavigatorSearchMessage: {message}'");

        // Split the SearchCodeOriginal into the tab and the search term, if present
        string searchCode = message.SearchCodeOriginal;
        string searchTerm = message.FilteringData;

        // Parse the search type based on the search code
        int searchType = ParseSearchType(searchCode);
        logger.LogDebug($"Parsed search type: {searchType} from SearchCodeOriginal: {searchCode}");

        // Send the search result based on the search type and search term (using FilteringData or searchTerm)
        await navigatorManager.SendGuestRoomSearchResult(session.Player, searchType, searchTerm);
    }
    
    private int ParseSearchType(string searchCode)
    {
        if (string.IsNullOrEmpty(searchCode))
        {
            logger.LogWarning("Received a null or empty search code. Defaulting to 0.");
            return 0;
        }

        // Map searchCode to the appropriate NavigatorSearchType integer
        return searchCode.ToLower() switch
        {
            "popular" => (int)NavigatorSearchType.PopularRooms,
            "highest_score" => (int)NavigatorSearchType.RoomsWithHighestScore,
            "myworld_view" => (int)NavigatorSearchType.MyRooms,
            "hotel_view" => (int)NavigatorSearchType.PopularRooms,
            // Add additional mappings as necessary
            _ => 0 // Default or error value
        };
    }
}