using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Networking.Game.Codec;

public class EncryptionDecoder(ISession session) : ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        if (input.ReadableBytes == 0)
        {
            return;
        }

        var data = new byte[input.ReadableBytes];
        input.ReadBytes(data);

        var decryptedData = session.Rc4.Decrypt(data);

        if (decryptedData.Length <= 0) return;
        
        output.Add(Unpooled.WrappedBuffer(decryptedData));
    }
}