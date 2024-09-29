using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Navigator;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Session;
using Turbo.Packets.Shared.Navigator;
using Turbo.Rooms.Utils;

namespace Turbo.Navigator;

public class NavigatorManager(
    IRoomManager _roomManager,
    ILogger<INavigatorManager> _logger,
    IServiceScopeFactory _serviceScopeFactory) : Component, INavigatorManager
{
    private readonly IDictionary<int, INavigatorCategory> _categories = new Dictionary<int, INavigatorCategory>();

    private readonly IDictionary<int, INavigatorEventCategory> _eventCategories =
        new Dictionary<int, INavigatorEventCategory>();

    private readonly IDictionary<int, INavigatorCollapsedCategories> _collapsedCategories =
        new Dictionary<int, INavigatorCollapsedCategories>();

    private readonly ConcurrentDictionary<int, IPendingRoomInfo> _pendingRoomIds = new();
    private readonly IList<INavigatorTopLevelContext> _tabs = new List<INavigatorTopLevelContext>();

    private readonly ConcurrentDictionary<int, FavoriteRoomsCacheItem> _favoriteRoomCache = new();


    //TODO: Add this to a configuration table
    private const int MaxFavoriteRooms = 30;

    public async Task CreateFlat(IPlayer player, string name, string description, string modelName, int maxUsers, int categoryId, RoomTradeType tradeType)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var navigatorRepository = scope.ServiceProvider.GetRequiredService<INavigatorRepository>();

        // TODO: Check if the player has already exceeded the room creation limit.
        // Note: Subscription levels may affect or alter this limit.

        var flatCategoryEntity = await navigatorRepository.FlatCategoryEntityByIdAsync(categoryId);

        if (flatCategoryEntity == null)
        {
            _logger.LogError("Unidentified flat category entity with ID '{categoryId}'", categoryId);
        }

        var room = await _roomManager.CreateRoom(player, name, description, modelName, maxUsers, categoryId, tradeType);

        await player.Session.Send(new FlatCreatedMessage
        {
            RoomId = room.Id,
            RoomName = room.RoomDetails.Name
        });
    }

    public int GetPendingRoomId(int userId) => _pendingRoomIds.TryGetValue(userId, out var info) ? info.RoomId : -1;

    public void SetPendingRoomId(int userId, int roomId, bool approved = false)
    {
        if (userId <= 0 || roomId <= 0) return;

        _pendingRoomIds.AddOrUpdate(
            userId,
            new PendingRoomInfo { RoomId = roomId, Approved = approved },
            (key, existingVal) => new PendingRoomInfo { RoomId = roomId, Approved = approved }
        );
    }

    public void ClearPendingRoomId(int userId) => _pendingRoomIds.Remove(userId, out var pendingRoomInfo);

    public void ClearRoomStatus(IPlayer player)
    {
        if (player == null) return;

        ClearPendingDoorbell(player);

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == -1) player.Session?.Send(new CloseConnectionMessage());
    }

    public async Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false)
    {
        if (player == null) return;

        var room = await _roomManager.GetRoom(roomId);

        if (room == null) return;

        await player.Session.Send(new GetGuestRoomResultMessage
        {
            EnterRoom = enterRoom,
            Room = room,
            IsRoomForward = roomForward,
            IsStaffPick = false,
            IsGroupMember = false,
            AllInRoomMuted = false,
            CanMute = false
        });
    }

    public async Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false,
        IPoint location = null)
    {
        if (player == null || roomId <= 0) return;

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == roomId) return;

        SetPendingRoomId(player.Id, roomId);

        player.ClearRoomObject();

        var room = await _roomManager.GetRoom(roomId);

        if (room != null) await room.InitAsync();

        if (room == null || room.RoomModel == null)
        {
            ClearPendingRoomId(player.Id);

            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed,
                Parameter = ""
            });

            return;
        }

        if (!room.RoomSecurityManager.IsOwner(player))
        {
            if (room.RoomSecurityManager.IsPlayerBanned(player))
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Banned,
                    Parameter = ""
                });

                return;
            }

            if (room.RoomDetails.UsersNow >= room.RoomDetails.UsersMax)
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Full,
                    Parameter = ""
                });

                return;
            }

            if (!skipState)
            {
                #region RoomStateType.Locked

                if (room.RoomDetails.State == RoomStateType.Locked)
                {
                    ClearPendingRoomId(player.Id);

                    // doorbell
                    // if rights do u need 2 wait
                }

                #endregion
                #region RoomStateType.Password

                else if (room.RoomDetails.State == RoomStateType.Password)
                {
                    if (!password.Equals(room.RoomDetails.Password))
                    {
                        ClearPendingRoomId(player.Id);

                        await player.Session.Send(new GenericErrorMessage
                        {
                            ErrorCode = RoomGenericErrorType.InvalidPassword
                        });

                        return;
                    }
                }

                #endregion
                #region RoomStateType.Invisible

                else if (room.RoomDetails.State == RoomStateType.Invisible)
                {
                    if (room.RoomSecurityManager.GetControllerLevel(player) == RoomControllerLevel.None)
                    {
                        ClearPendingRoomId(player.Id);

                        await player.Session.Send(new CantConnectMessage
                        {
                            Reason = CantConnectReason.Closed,
                            Parameter = ""
                        });

                        return;
                    }
                }

                #endregion
            }
        }

        ClearPendingDoorbell(player);

        SetPendingRoomId(player.Id, roomId, true);

        using var scope = _serviceScopeFactory.CreateScope();
        var roomEnterLogRepository = scope.ServiceProvider.GetRequiredService<IRoomEntryLogRepository>();

        await roomEnterLogRepository.AddRoomEntryLogAsync(roomId, player.Id);

        //Is this not send when creating a room?
        await player.Session.Send(new FlatAccessibleMessage
        {
            RoomId = roomId,
            Username = player.Name
        });

        if (location != null) _pendingRoomIds[player.Id].Location = new Point(location);

        await player.Session.Send(new OpenConnectionMessage
        {
            RoomId = roomId
        });
        await player.Session.Send(new RoomReadyMessage
        {
            RoomId = room.Id,
            RoomType = room.RoomModel.Name
        });

        await ContinueEnteringRoom(player);
    }

    public async Task ContinueEnteringRoom(IPlayer player)
    {
        if (player == null) return;

        if (!_pendingRoomIds.ContainsKey(player.Id) || !_pendingRoomIds[player.Id].Approved)
        {
            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed
            });

            return;
        }

        var roomId = _pendingRoomIds[player.Id].RoomId;

        var room = await _roomManager.GetRoom(roomId);

        if (room == null)
            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed
            });

        if (room != null)
        {
            await room.InitAsync();

            room.EnterRoom(player, _pendingRoomIds[player.Id].Location);
        }

        ClearPendingRoomId(player.Id);
    }

    public async Task SendNavigatorCategories(IPlayer player)
    {
        await player.Session.Send(new UserEventCatsMessage
        {
            EventCategories = [.. _eventCategories.Values]
        });
        await player.Session.Send(new UserFlatCatsMessage
        {
            Categories = [.. _categories.Values]
        });
    }

    public async Task SendNavigatorSettings(IPlayer player) => await player.Session.Send(new NewNavigatorPreferencesMessage
    {
        WindowX = 0,
        WindowY = 0,
        WindowWidth = 0,
        WindowHeight = 0,
        LeftPaneHidden = false,
        ResultMode = 0
    });

    public async Task SendNavigatorMetaData(IPlayer player)
    {
        if (_tabs == null || _tabs.Count == 0)
        {
            _logger.LogError("No Navigator tabs found.");
            return;
        }

        var topLevelContexts = _tabs.Select(tab => new TopLevelContext
        {
            SearchCode = tab.SearchCode,
            SavedSearches = new List<INavigatorSavedSearch>()
        }).ToList<ITopLevelContext>();

        await player.Session.Send(new NavigatorMetaDataMessage
        {
            TopLevelContexts = topLevelContexts
        });
    }

    public async Task SendNavigatorLiftedRooms(IPlayer player) => await player.Session.Send(new NavigatorLiftedRoomsMessage
    {
        LiftedRooms =
        [
            new LiftedRoom
            {
                FlatId = 1,
                Unused = 0,
                Image = "",
                Caption = ""
            }
        ]
    });

    public async Task SendNavigatorSavedSearches(IPlayer player) => await player.Session.Send(new NavigatorSavedSearchesMessage
    {
        // Todo: Implement saved searches
        SavedSearches = []
    });

    public async Task SendNavigatorEventCategories(IPlayer player) => await player.Session.Send(new NavigatorEventCategoriesMessage
    {
        EventCategories = [.. _eventCategories.Values]
    });

    public async Task SendNavigatorCollapsedCategories(IPlayer player)
    {
        if (_collapsedCategories.TryGetValue(player.Id, out var collapsedCategories))
        {
            await player.Session.Send(new NavigatorCollapsedCategoriesMessage
            {
                CollapsedCategories = collapsedCategories.CollapsedCategories
            });
        }
        else
        {
            await player.Session.Send(new NavigatorCollapsedCategoriesMessage
            {
                CollapsedCategories = []
            });
        }
    }

    public async Task SendGuestRoomSearchResult(IPlayer player, int searchType, string searchParam)
    {
        var rooms = await _roomManager.SearchRooms(searchParam);

        var results = new List<ISearchResultData>
        {
            new SearchResultData
            {
                SearchCode = searchParam,
                Text = "Search Results",
                ActionAllowed = 0,
                ForceClosed = false,
                ViewMode = 0,
                Rooms = rooms
            }
        };

        var message = new NavigatorSearchResultBlocksMessage
        {
            SearchCode = searchParam,
            Filtering = searchParam,
            Results = results
        };

        await player.Session.Send(message);
    }

    protected override async Task OnInit() => await LoadNavigatorData();

    protected override async Task OnDispose()
    {
    }

    public void ClearPendingDoorbell(IPlayer player)
    {
        if (player == null) return;

        // remove user from pending doorbells
    }

    private async Task LoadNavigatorData()
    {
        _tabs.Clear();
        _categories.Clear();
        _eventCategories.Clear();

        using var scope = _serviceScopeFactory.CreateScope();
        var navigatorRepository = scope.ServiceProvider.GetRequiredService<INavigatorRepository>();

        var tabEntities = await navigatorRepository.GetTopLevelContextsAsync();
        var flatCategoryEntities = await navigatorRepository.GetFlatCategoriesAsync();
        var eventCategoryEntities = await navigatorRepository.GetEventCategoriesAsync();

        tabEntities.ForEach(entity =>
        {
            var tab = new NavigatorTopLevelContext(entity);

            _tabs.Add(tab);
        });

        flatCategoryEntities.ForEach(entity =>
        {
            var category = new NavigatorFlatCategory(entity);

            _categories.Add(category.Id, category);
        });

        eventCategoryEntities.ForEach(entity =>
        {
            var eventCategory = new NavigatorEventCategory(entity);

            _eventCategories.Add(eventCategory.Id, eventCategory);
        });

        _logger.LogInformation("Loaded {0} navigator tabs", _tabs.Count);
        _logger.LogInformation("Loaded {0} navigator categories", _categories.Count);
        _logger.LogInformation("Loaded {0} navigator event categories", _eventCategories.Count);
    }

    public async Task HandleNavigatorSearch(IPlayer player, string searchCode, string searchParam, string filterMode)
    {
        _logger.LogInformation("HandleNavigatorSearch called with searchCode: {searchCode}, searchParam: {searchParam}, filterMode: {filterMode}", searchCode, searchParam, filterMode);

        switch (searchCode)
        {
            case "official_view":
                await SendOfficialRooms(player);
                break;
            case "hotel_view":
                await SendHotelView(player);
                break;
            case "myworld_view":
                await SendMyWorldView(player, searchParam, filterMode);
                break;
            case "category":
                await SendCategoryRooms(player, searchParam);
                break;
            default:
                _logger.LogWarning("Unhandled searchCode: {searchCode}", searchCode);
                await SendEmptySearchResults(player, searchCode);
                break;
        }
    }

    public async Task SendOfficialRooms(IPlayer player)
    {
        var categoryKeys = new[] { "OFFICIAL", "NEW", "RoomBundles" };
        var categories = _categories.Values
            .Where(c => categoryKeys.Contains(c.GlobalCategoryKey))
            .OrderBy(c => c.OrderNum)
            .ToList();

        var categoryIds = categories.Select(c => c.Id).ToList();

        // Fetch all rooms for the categories in a single call
        var rooms = await _roomManager.GetRoomsByCategoriesAsync(categoryIds);

        // Group rooms by category
        var roomsByCategory = rooms.GroupBy(r => r.RoomDetails.CategoryId);

        var results = new List<ISearchResultData>();

        foreach (var category in categories)
        {
            var roomsInCategory = roomsByCategory.FirstOrDefault(g => g.Key == category.Id)?.ToList();

            if (roomsInCategory != null && roomsInCategory.Any())
            {
                results.Add(new SearchResultData
                {
                    SearchCode = "official_view",
                    Text = category.Name,
                    ActionAllowed = 0,
                    ForceClosed = false,
                    ViewMode = 0,
                    Rooms = roomsInCategory
                });
            }
        }

        var message = new NavigatorSearchResultBlocksMessage
        {
            SearchCode = "official_view",
            Filtering = "",
            Results = results
        };

        await player.Session.Send(message);
    }

    public async Task SendHotelView(IPlayer player)
    {
        // Fetch most popular rooms
        var popularRoomsTask = _roomManager.GetRoomsOrderedByPopularityAsync();

        // Fetch categories associated with the 'All Rooms' tab
        var categories = _categories.Values
            .Where(c => c.GlobalCategoryKey != "OFFICIAL" && c.GlobalCategoryKey != "NEW" && c.GlobalCategoryKey != "RoomBundles")
            .OrderBy(c => c.OrderNum)
            .ToList();

        var categoryIds = categories.Select(c => c.Id).ToList();

        // Fetch rooms for all categories in one call
        var roomsByCategoryTask = _roomManager.GetRoomsByCategoriesAsync(categoryIds);

        // Wait for both tasks to complete concurrently
        await Task.WhenAll(popularRoomsTask, roomsByCategoryTask);

        var popularRooms = popularRoomsTask.Result;
        var roomsByCategory = roomsByCategoryTask.Result;

        // Group rooms by their category ID
        var roomsGroupedByCategoryId = roomsByCategory
            .GroupBy(r => r.RoomDetails.CategoryId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var results = new List<ISearchResultData>
        {
            new SearchResultData
            {
                SearchCode = "hotel_view",
                Text = "Most Popular Rooms",
                ActionAllowed = 0,
                ForceClosed = false,
                ViewMode = 0,
                Rooms = popularRooms
            }
        };

        // You may also add "Recommended For You" here if applicable

        foreach (var category in categories)
        {
            if (roomsGroupedByCategoryId.TryGetValue(category.Id, out var rooms) && rooms.Any())
            {
                results.Add(new SearchResultData
                {
                    SearchCode = "category__" + category.GlobalCategoryKey.ToLower(),
                    Text = category.Name,
                    ActionAllowed = 0,
                    ForceClosed = false,
                    ViewMode = 0,
                    Rooms = rooms
                });
            }
        }

        var message = new NavigatorSearchResultBlocksMessage
        {
            SearchCode = "hotel_view",
            Filtering = "",
            Results = results
        };

        await player.Session.Send(message);
    }

    public async Task SendMyWorldView(IPlayer player, string? searchParam = null, string? filterMode = "anything")
    {
        // Fetch data concurrently
        var myRoomsTask = _roomManager.GetRoomsByOwnerAsync(player.Id, searchParam, filterMode);
        var favoriteRoomsTask = _roomManager.GetFavoriteRoomsAsync(player.Id);
        var roomsHistoryTask = _roomManager.GetRoomsHistoryAsync(player.Id, searchParam, filterMode);
        //var rightsRoomsTask = _roomManager.GetRoomsWithRightsAsync(player.Id);

        await Task.WhenAll(myRoomsTask, favoriteRoomsTask, roomsHistoryTask);

        // Retrieve results
        var myRooms = myRoomsTask.Result;
        var favoriteRooms = favoriteRoomsTask.Result;
        var roomsHistory = roomsHistoryTask.Result;
        //var rightsRooms = rightsRoomsTask.Result;

        // Prepare search result blocks
        var results = new List<ISearchResultData>();

        if (myRooms.Any())
            results.Add(CreateSearchResultData("my_rooms", "My Rooms", myRooms));

        if (favoriteRooms.Any())
            results.Add(CreateSearchResultData("favorites", "My Favourite Rooms", favoriteRooms));

        if (roomsHistory.Any())
            results.Add(CreateSearchResultData("history", "My Room Visit History", roomsHistory));

        //if (rightsRooms.Any())
        //results.Add(CreateSearchResultData("with_rights", "Rooms where I have rights", rightsRooms));

        var message = new NavigatorSearchResultBlocksMessage
        {
            SearchCode = "myworld_view",
            Filtering = "",
            Results = results
        };

        await player.Session.Send(message);
    }

    public async Task SendCategoryRooms(IPlayer player, string searchParam)
    {
        var categoryKey = searchParam;

        if (string.IsNullOrEmpty(categoryKey))
        {
            await SendEmptySearchResults(player, "category");
            return;
        }

        var category = _categories.Values.FirstOrDefault(c => c.GlobalCategoryKey.Equals(categoryKey, StringComparison.OrdinalIgnoreCase));

        if (category != null)
        {
            // Create a list containing the single category ID
            var categoryIds = new List<int> { category.Id };
            var rooms = await _roomManager.GetRoomsByCategoriesAsync(categoryIds);

            if (rooms.Count != 0)
            {
                var results = new List<ISearchResultData>
                {
                    new SearchResultData
                    {
                        SearchCode = "category",
                        Text = category.Name,
                        ActionAllowed = 0,
                        ForceClosed = false,
                        ViewMode = 0,
                        Rooms = rooms
                    }
                };

                var message = new NavigatorSearchResultBlocksMessage
                {
                    SearchCode = "category",
                    Filtering = category.GlobalCategoryKey.ToLower(),
                    Results = results
                };

                await player.Session.Send(message);
            }
            else
            {
                await SendEmptySearchResults(player, "category");
            }
        }
        else
        {
            await SendEmptySearchResults(player, "category");
        }
    }

    private ISearchResultData CreateSearchResultData(string searchCode, string text, IList<IRoom> rooms) => new SearchResultData
    {
        SearchCode = searchCode,
        Text = text,
        ActionAllowed = 0, // Adjust if necessary
        ForceClosed = false,
        ViewMode = 0, // Adjust if necessary
        Rooms = rooms
    };

    private async Task SendEmptySearchResults(IPlayer player, string searchCode)
    {
        var message = new NavigatorSearchResultBlocksMessage
        {
            SearchCode = searchCode,
            Filtering = "",
            Results = new List<ISearchResultData>()
        };

        await player.Session.Send(message);
    }

    public async Task HandleFavouriteRoomChangeAsync(int playerId, int roomId, bool isAdding)
    {
        try
        {
            var roomExists = await _roomManager.RoomExistsAsync(roomId);
            if (!roomExists)
            {
                _logger.LogWarning("Player {PlayerId} tried to add non-existent Room {RoomId} to favorites.", playerId, roomId);
                // Optionally, send an error message to the player
                return;
            }

            var favoriteRooms = await GetOrCreateFavoriteRoomsCacheAsync(playerId);

            if (isAdding)
            {
                if (favoriteRooms.Count >= MaxFavoriteRooms)
                {
                    _logger.LogWarning("Player {PlayerId} has reached the maximum number of favorite rooms.", playerId);
                    // Optionally, send a message to the player indicating they've reached the limit
                    return;
                }

                if (favoriteRooms.TryAdd(roomId, 0))
                {
                    // Update the database
                    using var scope = _serviceScopeFactory.CreateScope();
                    var favouriteRoomsRepository = scope.ServiceProvider.GetRequiredService<IPlayerFavouriteRoomsRepository>();
                    await favouriteRoomsRepository.AddFavoriteRoomAsync(playerId, roomId);
                }
            }
            else
            {
                if (favoriteRooms.TryRemove(roomId, out _))
                {
                    // Update the database
                    using var scope = _serviceScopeFactory.CreateScope();
                    var favouriteRoomsRepository = scope.ServiceProvider.GetRequiredService<IPlayerFavouriteRoomsRepository>();
                    await favouriteRoomsRepository.RemoveFavoriteRoomAsync(playerId, roomId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating favorite rooms for PlayerId: {PlayerId}, RoomId: {RoomId}, IsAdding: {IsAdding}", playerId, roomId, isAdding);
            throw;
        }
    }

    public async Task<ConcurrentDictionary<int, byte>> GetFavoriteRoomsAsync(int playerId) => await GetOrCreateFavoriteRoomsCacheAsync(playerId);


    private async Task<ConcurrentDictionary<int, byte>> GetOrCreateFavoriteRoomsCacheAsync(int playerId)
    {
        if (_favoriteRoomCache.TryGetValue(playerId, out var cacheItem))
        {
            if (cacheItem.Expiration > DateTime.UtcNow)
            {
                return cacheItem.FavoriteRooms;
            }
            else
            {
                // Remove expired cache item
                _favoriteRoomCache.TryRemove(playerId, out _);
            }
        }

        // Load from database
        using var scope = _serviceScopeFactory.CreateScope();
        var favouriteRoomsRepository = scope.ServiceProvider.GetRequiredService<IPlayerFavouriteRoomsRepository>();

        var roomIds = await favouriteRoomsRepository.GetFavoriteRoomsAsync(playerId);
        var favoriteRooms = new ConcurrentDictionary<int, byte>(roomIds.Select(id => new KeyValuePair<int, byte>(id, 0)));

        // Set cache expiration (e.g., 30 minutes)
        var expiration = DateTime.UtcNow.AddMinutes(30);
        cacheItem = new FavoriteRoomsCacheItem
        {
            FavoriteRooms = favoriteRooms,
            Expiration = expiration
        };

        _favoriteRoomCache[playerId] = cacheItem;

        return favoriteRooms;
    }

    public async Task LoadFavoriteRoomsCacheAsync(int playerId) => await GetOrCreateFavoriteRoomsCacheAsync(playerId);

    private (string key, string value) ParseSearchParam(string searchParam)
    {
        if (string.IsNullOrEmpty(searchParam))
        {
            _logger.LogError("Error parsing navigator searchParam: null value");
        }

        var parts = searchParam?.Split(new[] { ':' }, 2);

        if (parts!.Length < 2)
        {
            _logger.LogError("Error parsing navigator searchParam: wrong length");
        }

        var key = parts[0].Trim();
        var value = parts[1].Trim();

        return (key.ToLower(), value);
    }
}