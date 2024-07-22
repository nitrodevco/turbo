using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Revisions;
using Turbo.Packets.Incoming.Handshake;

namespace Turbo.Packets.Revisions;

public class RevisionManager : IRevisionManager
{
    private readonly ILogger<IRevisionManager> _logger;

    private readonly IPacketMessageHub _packetMessageHub;

    public RevisionManager(ILogger<IRevisionManager> logger, IPacketMessageHub messageHub)
    {
        _packetMessageHub = messageHub;
        _logger = logger;

        Revisions = new Dictionary<string, IRevision>();

        _packetMessageHub.Subscribe<ClientHelloMessage>(this, OnRevisionMessage);
    }

    public IDictionary<string, IRevision> Revisions { get; }
    public IRevision DefaultRevision { get; set; }

    public Task OnRevisionMessage(ClientHelloMessage message, ISession session)
    {
        if (Revisions.TryGetValue(message.Production, out var revision))
            session.Revision = revision;
        else
            _logger.LogDebug($"No matching revision implementation found for {message.Production}");

        return Task.CompletedTask;
    }
}