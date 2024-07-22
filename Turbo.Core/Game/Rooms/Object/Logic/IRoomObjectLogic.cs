using System;
using Turbo.Core.Events;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IRoomObjectLogic : IDisposable, ICyclable
{
    public bool OnReady();
    public void SetEventHub(ITurboEventHub eventHub);
}