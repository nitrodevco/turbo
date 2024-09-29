using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record FavouritesMessage(int Limit,
    List<int> FavouriteRoomIds) : IComposer;