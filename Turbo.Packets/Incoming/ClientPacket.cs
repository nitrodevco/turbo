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
            return string.Empty;

        byte[] data = new byte[length];
        Content.ReadBytes(data); // Read the exact number of bytes into the array

        return Encoding.UTF8.GetString(data); // Convert bytes to string
    }

    public int PopInt()
    {
        return Content.ReadInt();
    }

    public bool PopBoolean()
    {
        return Content.ReadByte() == 1;
    }

    public int RemainingLength()
    {
        return Content.ReadableBytes;
    }

    public long PopLong()
    {
        return Content.ReadLong();
    }

    public short PopShort()
    {
        return Content.ReadShort();
    }
    
    public ushort PopUShort()
    {
        return Content.ReadUnsignedShort();
    }

    public double PopDouble()
    {
        var doubleString = PopString();
        var parsed = double.TryParse(doubleString, out var result);

        if (parsed)
            return result;

        throw new FormatException($"'{doubleString}' is not a valid double!");
    }
}