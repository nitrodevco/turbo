using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired;

public record WiredValidationErrorMessage : IComposer
{
    public string Info { get; init; }
}