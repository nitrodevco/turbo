using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record FavouriteChangedMessage(int RoomId,
    bool Added) : IComposer;