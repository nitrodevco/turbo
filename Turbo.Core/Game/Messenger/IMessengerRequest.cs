namespace Turbo.Core.Game.Messenger;

public interface IMessengerRequest
{
    int Id { get; }
    string Name { get; }
    string Figure { get; }
}
