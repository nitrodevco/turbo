using System;

namespace Turbo.Core.Game.Messenger;

public interface IMessengerConsoleMessage
{
    int Id { get; }
    int ChatId { get; }
    string MessageText { get; }
    DateTime SentAt { get; }
    int SecondsSinceSent { get; }
    int ConfirmationId { get; }
    int SenderId { get; }
    string SenderName { get; }
    string SenderFigure { get; }
    int RecipientId { get; }
    string RecipientName { get; }
    string RecipientFigure { get; }
}
