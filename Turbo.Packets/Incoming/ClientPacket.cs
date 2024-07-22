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
        int length = Content.ReadShort();
        var data = Content.ReadBytes(length);
        return Encoding.UTF8.GetString(data.Array);
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

    public double PopDouble()
    {
        var doubleString = PopString();
        var parsed = double.TryParse(doubleString, out var result);

        if (parsed)
            return result;

        throw new FormatException($"'{doubleString}' is not a valid double!");
    }
}