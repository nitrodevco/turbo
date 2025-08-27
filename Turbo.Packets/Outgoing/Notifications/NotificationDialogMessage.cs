using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Notifications;
public class NotificationDialogMessage : IComposer
{
    public required string Type { get; init; }
    public IDictionary<string, string>? Keys { get; init; }
}
