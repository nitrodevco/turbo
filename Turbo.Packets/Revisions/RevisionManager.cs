using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Sessions;

namespace Turbo.Packets.Revisions
{
    public class RevisionManager : IRevisionManager
    {
        public IDictionary<string, IRevision> Revisions { get; }
        public IRevision DefaultRevision { get; set; }

        private readonly IPacketMessageHub _packetMessageHub;
        private readonly ILogger<IRevisionManager> _logger;

        public RevisionManager(ILogger<IRevisionManager> logger, IPacketMessageHub messageHub)
        {
            this._packetMessageHub = messageHub;
            this._logger = logger;

            this.Revisions = new Dictionary<string, IRevision>();

            _packetMessageHub.Subscribe<ClientHelloMessage>(this, OnRevisionMessage);
        }

        public Task OnRevisionMessage(ClientHelloMessage message, ISession session)
        {
            if (Revisions.TryGetValue(message.Production, out IRevision revision))
            {
                session.Revision = revision;
            }
            else
            {
                _logger.LogDebug($"No matching revision implementation found for {message.Production}");
            }

            return Task.CompletedTask;
        }
    }
}
