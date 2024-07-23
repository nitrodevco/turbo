using DotNetty.Buffers;

namespace Turbo.Core.Packets.Messages;

public interface IClientPacket
{
    IByteBuffer Content { get; }
    int Header { get; }
    string PopString();
    int PopInt();
    bool PopBoolean();
    short PopShort();
    double PopDouble();
    long PopLong();
    int RemainingLength();
}