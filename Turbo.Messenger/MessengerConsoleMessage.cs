using Turbo.Core.Game.Messenger;
using Turbo.Database.Entities.Logs.History;

namespace Turbo.Messenger;
public class MessengerConsoleMessage(
    int chatId,
    int secondsSinceSent,
    ConsoleChatLogEntity entity,
    int? confirmationId = null
) : IMessengerConsoleMessage
{
    public int Id => entity.Id;
    public int ChatId => chatId;
    public string MessageText => entity.Message;
    public DateTime SentAt => entity.SentAt;
    public int SecondsSinceSent => secondsSinceSent;
    public int ConfirmationId => confirmationId ?? entity.ConfirmationId;
    public int SenderId => entity.SenderEntityId;
    public string SenderName => entity.SenderEntity.Name;
    public string SenderFigure => entity.SenderEntity.Figure;
    public int RecipientId => entity.RecipientEntityId;
    public string RecipientName => entity.RecipientEntity.Name;
    public string RecipientFigure => entity.RecipientEntity.Figure;
}
