using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Notifications;

public class InfoFeedEnableMessage : IComposer
{
    public bool Enabled { get; set; }
}