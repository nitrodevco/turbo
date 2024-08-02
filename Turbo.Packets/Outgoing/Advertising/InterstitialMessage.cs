using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Advertising;

public class InterstitialMessage : IComposer
{
    public string imageUrl { get; set; }
    public string clickUrl { get; set; }
}