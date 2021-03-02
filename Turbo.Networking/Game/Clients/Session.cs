using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Turbo.Core.Players;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Revisions;
using Turbo.Packets.Serializers;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Game.Clients
{
    public class Session : ISession
    {
        private readonly IChannelHandlerContext _channel;

        public IPlayer Player { get; set; }

        public string IPAddress { get; set; }

        public IRevision Revision { get; set; }

        public Session(IChannelHandlerContext channel, IRevision initialRevision)
        {
            this._channel = channel;
            this.Revision = initialRevision;
        }

        public void Disconnect()
        {
            _channel.CloseAsync();
        }

        public ISession Send(IComposer composer)
        {
            Send(composer, false);
            return this;
        }

        public ISession SendQueue(IComposer composer)
        {
            Send(composer, true);
            return this;
        }

        protected void Send(IComposer composer, bool queue)
        {
            if(Revision.Serializers.TryGetValue(composer.GetType(), out ISerializer serializer))
            {
                IServerPacket packet = serializer.Serialize(_channel.Allocator.Buffer(2), composer);
                if (queue) _channel.WriteAsync(packet);
                else _channel.WriteAndFlushAsync(packet);
            }
        }

        public void Flush() => _channel.Flush();
    }
}
