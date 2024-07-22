using System;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Revisions;

namespace Turbo.Networking.Game.Clients;

public class SessionFactory : ISessionFactory
{
    private readonly IServiceProvider _provider;

    public SessionFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ISession Create(IChannelHandlerContext context, IRevision initialRevision)
    {
        var logger = _provider.GetService<ILogger<Session>>();
        return new Session(context, initialRevision, logger);
    }
}