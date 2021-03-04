using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Networking.Messages
{
    public interface IMessageDataWrapper
    {
        public byte ReadByte();
        public IByteBuffer ReadBytes(int length);
        public bool ReadBoolean();
        public short ReadShort();
        public int ReadInt();
        public string ReadString();

        public int Header { get; }
        public int BytesAvailable { get; }
    }
}
