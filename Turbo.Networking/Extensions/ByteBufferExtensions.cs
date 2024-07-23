using DotNetty.Buffers;

namespace Turbo.Networking.Extensions;

public static class ByteBufferExtensions
{
    public static bool ReleaseAll(this IByteBuffer buffer)
    {
        return buffer.ReferenceCount != 0 && buffer.Release(buffer.ReferenceCount);
    }
}