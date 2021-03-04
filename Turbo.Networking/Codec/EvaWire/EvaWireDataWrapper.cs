using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Networking.Messages;

namespace Turbo.Networking.Game.Codec
{
    public class EvaWireDataWrapper : IMessageDataWrapper
    {
        public int Header { get; private set; }

        private IByteBuffer _buffer;

        public EvaWireDataWrapper(int header, IByteBuffer buffer)
        {
            Header = header;

            _buffer = buffer;
        }

        public byte ReadByte()
        {
            return _buffer.ReadByte();
        }

        public IByteBuffer ReadBytes(int length)
        {
            return _buffer.ReadBytes(length);
        }

        public bool ReadBoolean()
        {
            return (ReadByte() == 1);
        }

        public short ReadShort()
        {
            return _buffer.ReadShort();
        }

        public int ReadInt()
        {
            return _buffer.ReadInt();
        }

        public string ReadString()
        {
            IByteBuffer buffer = ReadBytes(ReadShort());

            return Encoding.UTF8.GetString(buffer.Array);
        }

        public int BytesAvailable
        {
            get
            {
                return _buffer.ReadableBytes;
            }
        }
    }
}
