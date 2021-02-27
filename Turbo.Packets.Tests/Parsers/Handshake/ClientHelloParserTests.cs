using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Parsers;
using Turbo.Packets.Parsers.Handshake;
using Xunit;
using DotNetty.Buffers;
using AutoFixture;

namespace Turbo.Packets.Tests.Parsers.Handshake
{
    public class ClientHelloParserTests
    {
        private readonly IFixture _fixture;
        private readonly IParser<ClientHelloMessage> _sut;

        public ClientHelloParserTests()
        {
            _fixture = new Fixture();
            _sut = new ClientHelloParser();
        }

        [Fact]
        private void Parse_WithClientPacket_ReturnsClientHelloMessage()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var production = _fixture.Create<string>();
            var platform = _fixture.Create<string>();
            var clientPlatform = _fixture.Create<int>();
            var deviceCategory = _fixture.Create<int>();

            IByteBuffer buffer = Unpooled.Buffer();
            var encoding = Encoding.Default;

            buffer.WriteShort(encoding.GetByteCount(production));
            buffer.WriteString(production, encoding);
            buffer.WriteShort(encoding.GetByteCount(platform));
            buffer.WriteString(platform, encoding);
            buffer.WriteInt(clientPlatform);
            buffer.WriteInt(deviceCategory);

            var packet = new ClientPacket(packetHeader, buffer);

            // Act
            var result = _sut.Parse(packet);

            // Assert
            Assert.Equal(production, result.Production);
            Assert.Equal(platform, result.Platform);
            Assert.Equal(clientPlatform, result.ClientPlatform);
            Assert.Equal(deviceCategory, result.DeviceCategory);
        }
    }
}
