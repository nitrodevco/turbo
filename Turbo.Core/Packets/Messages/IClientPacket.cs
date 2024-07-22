namespace Turbo.Core.Packets.Messages;

public interface IClientPacket
{
    int Header { get; }
    string PopString();
    int PopInt();
    bool PopBoolean();
    short PopShort();
    double PopDouble();
    long PopLong();
    int RemainingLength();
}