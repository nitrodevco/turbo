using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System.Text;

namespace Turbo.Networking.Game.Handler
{
    class FlashPolicyHandler : ChannelHandlerAdapter
    {
        private static readonly string policy = "<?xml version=\"1.0\"?>\r\n"
                             + "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n"
                             + "<cross-domain-policy>\r\n"
                             + "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n"
                             + "</cross-domain-policy>\0)";
        public override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            bool release = true;
            try
            {
                IByteBuffer buf = (IByteBuffer)msg;
                if (buf.ReadableBytes < 1) return;

                if (buf.GetByte(0) == 0x3C)
                {
                    context.WriteAndFlushAsync(Unpooled.CopiedBuffer(Encoding.Default.GetBytes(policy))).ContinueWith(antecedent => context.CloseAsync());
                }
                else
                {
                    release = false;
                    context.FireChannelRead(msg);
                    context.Channel.Pipeline.Remove(this);
                }
            }
            finally
            {
                if (release)
                {
                    ReferenceCountUtil.Release(msg);
                }
            }
        }
    }
}
