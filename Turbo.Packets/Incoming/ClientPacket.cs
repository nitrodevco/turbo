using System;
using System.Text;
using DotNetty.Buffers;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming;

public class ClientPacket : TurboPacket, IClientPacket
{
    public ClientPacket(int header, IByteBuffer body) : base(header, body)
    {
    }

    public string PopString()
    {
        ushort length = Content.ReadUnsignedShort(); // Read the length as an unsigned short

        if (length == 0)
        {
            _log.Append("{s:\"\"}");
            return string.Empty;
        }

        byte[] data = new byte[length];
        Content.ReadBytes(data); // Read the exact number of bytes into the array
        string value = Encoding.UTF8.GetString(data); // Convert bytes to string

        _log.Append($"{{s:\"{value}\"}}");
        return value;
    }

    public int PopInt()
    {
        int value = Content.ReadInt();
        _log.Append($"{{i:{value}}}");
        return value;
    }

    public bool PopBoolean()
    {
        bool value = Content.ReadByte() == 1;
        _log.Append($"{{b:{value.ToString().ToLower()}}}");
        return value;
    }

    public int RemainingLength()
    {
        return Content.ReadableBytes;
    }

    public long PopLong()
    {
        long value = Content.ReadLong();
        _log.Append($"{{l:{value}}}");
        return value;
    }

    public short PopShort()
    {
        short value = Content.ReadShort();
        _log.Append($"{{h:{value}}}");
        return value;
    }

    public ushort PopUShort()
    {
        ushort value = Content.ReadUnsignedShort();
        _log.Append($"{{h:{value}}}");
        return value;
    }

    public double PopDouble()
    {
        var doubleString = PopString();
        var parsed = double.TryParse(doubleString, out var result);

        if (parsed)
        {
            _log.Append($"{{d:{result}}}");
            return result;
        }

        throw new FormatException($"'{doubleString}' is not a valid double!");
    }
}