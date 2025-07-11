using Microsoft.EntityFrameworkCore.Internal;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;
public record RoomRatingMessage : IComposer
{
    public int CurrentScore { get; init; }
    public bool CanRate { get; init; }
}
