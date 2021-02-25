using DotNetty.Codecs.Http;
using DotNetty.Transport.Channels;
using System;

namespace Turbo.Networking.REST.Handler
{
    class RestMessageHandler : SimpleChannelInboundHandler<IFullHttpRequest>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, IFullHttpRequest msg)
        {
            if(msg.Method.Equals(HttpMethod.Get))
            {

            }
            Console.WriteLine("Received http message " + msg.Method + msg.ToString());
        }
    }
}
