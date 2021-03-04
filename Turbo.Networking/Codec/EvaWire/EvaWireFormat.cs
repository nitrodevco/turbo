using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Messages;

namespace Turbo.Networking.Codec.EvaWire
{
    public class EvaWireFormat : ICodec
    {
        public List<IMessageDataWrapper> Decode(IByteBuffer buffer)
        {
            List<IMessageDataWrapper> wrappers = new List<IMessageDataWrapper>();

            while(true)
            {
                if (buffer.ReadableBytes < 6) return wrappers;

                int length = buffer.ReadInt();

                if (length < 2) return wrappers;

                if (buffer.ReadableBytes < length) return wrappers;

                IByteBuffer extracted = buffer.ReadBytes(length);

                wrappers.Add(new EvaWireDataWrapper(extracted.ReadShort(), extracted));
            }
        }
    }
}
