using System;
using System.Collections.Concurrent;

namespace Turbo.Navigator;

public class FavoriteRoomsCacheItem
{
    public ConcurrentDictionary<int, byte> FavoriteRooms { get; set; }
    public DateTime Expiration { get; set; }
}