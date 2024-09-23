using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object;

public class RoomObjectContainer<T>(Action<T> onRemove) : IRoomObjectContainer<T>
    where T : IRoomObject
{
    private int _counter;

    public ConcurrentDictionary<int, T> RoomObjects { get; } = new();

    public bool AddRoomObject(T roomObject)
    {
        if (roomObject == null) return false;

        return RoomObjects.TryAdd(roomObject.Id, roomObject);
    }

    public T GetRoomObject(int id)
    {
        if (id >= 0 && RoomObjects.TryGetValue(id, out var roomObject)) return roomObject;

        return default;
    }

    public void RemoveRoomObject(params int[] ids)
    {
        foreach (var id in ids) RemoveRoomObject(GetRoomObject(id));
    }

    public void RemoveRoomObject(params T[] roomObjects)
    {
        foreach (var roomObject in roomObjects)
        {
            if (roomObject == null) continue;

            if (!RoomObjects.TryRemove(new KeyValuePair<int, T>(roomObject.Id, roomObject))) continue;

            onRemove?.Invoke(roomObject);
        }
    }

    public void RemoveAllRoomObjects()
    {
        RemoveRoomObject(RoomObjects.Values.ToArray());
    }

    public int GetNextId()
    {
        return ++_counter;
    }
}