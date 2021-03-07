using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Revisions;
using Turbo.Packets.Serializers;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Game.Clients
{
    public class Session : ISession
    {
        private readonly IChannelHandlerContext _channel;
        private readonly ILogger<Session> _logger;

        public IRevision Revision { get; set; }
        public ISessionPlayer SessionPlayer { get; private set; }

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
            if (SessionPlayer != null) await SessionPlayer.DisposeAsync();

            await _channel.CloseAsync();
        }

        public bool SetSessionPlayer(ISessionPlayer sessionPlayer)
        {
            if ((SessionPlayer != null) && (SessionPlayer != sessionPlayer)) return false;

            SessionPlayer = sessionPlayer;

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
                IServerPacket packet = serializer.Serialize(_channel.Allocator.Buffer(2), composer);
                if (queue) await _channel.WriteAsync(packet);
                else await _channel.WriteAndFlushAsync(packet);
            }
            else
            {
                _logger.LogDebug($"No matching serializer found for message {composer.GetType().Name}");
            }
        }

        public void Flush() => _channel.Flush();
    }
}
