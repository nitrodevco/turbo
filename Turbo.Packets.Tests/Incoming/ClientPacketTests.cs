using AutoFixture;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Xunit;

namespace Turbo.Packets.Tests.Incoming
{
    public class ClientPacketTests
    {
        private readonly IFixture _fixture;
        private readonly IByteBuffer _buffer;
        private readonly Encoding _encoding = Encoding.UTF8;

        public ClientPacketTests()
        {
            _fixture = new Fixture();
            _buffer = Unpooled.Buffer();
        }

        [Fact]
        private void PopString_WithStringLengthAndString_ShouldReadStringLengthAndReturnTheString()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetString = _fixture.Create<string>();

            _buffer.WriteShort(_encoding.GetByteCount(packetString));
            _buffer.WriteString(packetString, _encoding);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopString();

            // Assert
            Assert.IsType<string>(result);
            Assert.Equal(packetString, result);
        }

        [Fact]
        private void PopInt_WithInt_ShouldReturnInt()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetInt = _fixture.Create<int>();

            _buffer.WriteInt(packetInt);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopInt();

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(packetInt, result);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        private void PopBoolean_WithBoolean_ShouldReturnBoolean(byte value, bool expectedResult)
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            _buffer.WriteByte(value);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopBoolean();

            // Assert
            Assert.IsType<bool>(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        private void RemainingLength_ShouldReturnRemainingLength()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetString = _fixture.Create<string>();

            _buffer.WriteShort(_encoding.GetByteCount(packetString));
            _buffer.WriteString(packetString, _encoding);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.RemainingLength();

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(_buffer.ReadableBytes, result);
        }

        [Fact]
        private void PopLong_WithLong_ShouldReturnLong()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetLong = _fixture.Create<long>();

            _buffer.WriteLong(packetLong);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopLong();

            // Assert
            Assert.IsType<long>(result);
            Assert.Equal(packetLong, result);
        }

        [Fact]
        private void PopShort_WithShort_ShouldReturnShort()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetShort = _fixture.Create<short>();

            _buffer.WriteShort(packetShort);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopShort();

            // Assert
            Assert.IsType<short>(result);
            Assert.Equal(packetShort, result);
        }

        [Fact]
        private void PopDouble_WithDoubleStringLengthAndDoubleString_ShouldReadDoubleStringLengthAndReturnTheDouble()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();

            var packetDouble = _fixture.Create<double>();
            var packetDoubleString = packetDouble.ToString();

            _buffer.WriteShort(_encoding.GetByteCount(packetDoubleString));
            _buffer.WriteString(packetDoubleString, _encoding);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.PopDouble();

            // Assert
            Assert.IsType<double>(result);
            Assert.Equal(packetDouble, result);
        }

        [Fact]
        private void PopDouble_WithFaultyString_ShouldThrowFormatException()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packetString = _fixture.Create<string>();

            _buffer.WriteShort(_encoding.GetByteCount(packetString));
            _buffer.WriteString(packetString, _encoding);

            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            Action result = () => packet.PopDouble();

            // Assert
            var ex = Assert.Throws<FormatException>(result);
            Assert.Equal($"'{packetString}' is not a valid double!", ex.Message);
        }

        [Fact]
        private void Header_ReturnsThePacketsHeader()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var packet = new ClientPacket(packetHeader, _buffer);

            // Act
            var result = packet.Header;

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(result, packetHeader);
        }
    }
}
