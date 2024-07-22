using DotNetty.Codecs;

namespace Turbo.Networking.Game.Codec;

public class FrameLengthFieldDecoder : LengthFieldBasedFrameDecoder
{
    private static readonly int MAX_LENGTH = 500000;
    private static readonly int LENGTH_FIELD_OFFSET = 0;
    private static readonly int LENGTH_FIELD_LENGTH = 4; // 4 byte int
    private static readonly int LENGTH_ADJUSTMENT = 0;
    private static readonly int BYTES_TO_STRIP = 4;

    public FrameLengthFieldDecoder() : base(MAX_LENGTH, LENGTH_FIELD_OFFSET, LENGTH_FIELD_LENGTH, LENGTH_ADJUSTMENT,
        BYTES_TO_STRIP)
    {
    }
}