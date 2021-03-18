using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Packets.Revisions;

namespace Turbo.Networking.Game.Clients
{
    public class Session : ISession
    {
        private readonly IChannelHandlerContext _channel;
        private readonly ILogger<Session> _logger;

        public IRevision Revision { get; set; }
        public IPlayer Player { get; private set; }

        public string IPAddress { get; private set; }
        public long LastPongTimestamp { get; set; }

        public Session(IChannelHandlerContext channel, IRevision initialRevision, ILogger<Session> logger)
        {
            _channel = channel;
            _logger = logger;

            Revision = initialRevision;
            LastPongTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public async ValueTask DisposeAsync()
        {
            if (Player != null)
            {
                await Player.DisposeAsync();

                Player = null;
            }

            await _channel.CloseAsync();
        }

        public bool SetPlayer(IPlayer sessionPlayer)
        {
            if ((Player != null) && (Player != sessionPlayer)) return false;

            Player = sessionPlayer;

            return true;
        }

        public async Task Send(IComposer composer)
        {
            await Send(composer, false);
        }

        public async Task SendQueue(IComposer composer)
        {
            await Send(composer, true);
        }

        protected async Task Send(IComposer composer, bool queue)
        {
            if (Revision.Serializers.TryGetValue(composer.GetType(), out ISerializer serializer))
            {
                IServerPacket packet = serializer.Serialize(_channel.Allocator.Buffer(), composer);
                if (queue) await _channel.WriteAsync(packet);
                else await _channel.WriteAndFlushAsync(packet);
                _logger.LogDebug($"Sent {packet.Header}: {composer.GetType().Name}");
            }
            else
            {
                _logger.LogDebug($"No matching serializer found for message {composer.GetType().Name}");
            }
        }

        public void Flush() => _channel.Flush();
    }
}
