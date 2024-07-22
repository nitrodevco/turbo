using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record RoomPropertyMessage : IComposer
{
    /// <summary>
    ///     Known {string} values can be found in <see cref="Core.Game.Rooms.Utils.RoomPropertyType" />
    /// </summary>
    public string Property { get; init; }

    public string Value { get; init; }
}