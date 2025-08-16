using Turbo.Core.Game.Messenger;
using Turbo.Database.Entities.Messenger;

namespace Turbo.Messenger;

public class MessengerRequest(MessengerRequestEntity entity) : IMessengerRequest
{
    public int Id => entity.PlayerEntityId;

    public string Name => entity.PlayerEntity.Name;

    public string Figure => entity.PlayerEntity.Figure;
}
