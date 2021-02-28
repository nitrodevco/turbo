using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Turbo.Core.Players;
using Turbo.Packets.Composers;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Game.Clients
{
    public class Session : ISession
    {
        private readonly IChannelHandlerContext _channel;

        public IPlayer Player { get; set; }

        public string IPAddress { get; set; }

        public string Revision { get; set; }

        public Session(IChannelHandlerContext channel)
        {
            this._channel = channel;
        }

        public void Disconnect()
        {
            _channel.CloseAsync();
        }

        public ISession Send(IComposer composer)
        {
            _channel.WriteAndFlushAsync(composer);
            return this;
        }

        public ISession SendQueue(IComposer composer)
        {
            _channel.WriteAsync(composer);
            return this;
        }
    }
}
