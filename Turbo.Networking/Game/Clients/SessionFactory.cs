using System;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Revisions;

namespace Turbo.Networking.Game.Clients;

public class SessionFactory(IServiceProvider provider) : ISessionFactory
{
    public ISession Create(IChannelHandlerContext context, IRevision initialRevision)
    {
        var logger = provider.GetService<ILogger<Session>>();

        return ActivatorUtilities.CreateInstance<Session>(provider, context, initialRevision, logger);
    }
}