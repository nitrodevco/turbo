namespace Turbo.Core.Networking.Game.Clients;

public interface ISessionHolder
{
    public ISession Session { get; }
    public bool SetSession(ISession session);
}