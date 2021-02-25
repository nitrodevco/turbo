using DotNetty.Transport.Channels;
using System;
using Turbo.Packets.Incoming;

namespace Turbo.Networking.Game.Handler
{
    class GameMessageHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            // register the client
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            // unregister client
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IClientPacket msg)
        {
            Console.WriteLine("Received " + msg.Header);

            // do the thing where we get parser for the message
            // then publish message data to listeners
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            // log the error
        }
    }
}
