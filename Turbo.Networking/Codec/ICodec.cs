using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Networking.Messages;

namespace Turbo.Networking.Codec
{
    public interface ICodec
    {
        public List<IMessageDataWrapper> Decode(IByteBuffer buffer);
    }
}
