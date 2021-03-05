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
    public class SSOTicketParserTests
    {
        private readonly IFixture _fixture;
        private readonly IParser _sut;

        public SSOTicketParserTests()
        {
            _fixture = new Fixture();
            _sut = new SSOTicketParser();
        }

        [Fact]
        private void Parse_WithClientPacket_ReturnsSSOTicketMessage()
        {
            // Arrange
            var packetHeader = _fixture.Create<int>();
            var ssoTicket = _fixture.Create<string>();

            IByteBuffer buffer = Unpooled.Buffer();
            var encoding = Encoding.UTF8;

            buffer.WriteShort(encoding.GetByteCount(ssoTicket));
            buffer.WriteString(ssoTicket, encoding);

            var packet = new ClientPacket(packetHeader, buffer);

            // Act
            var result = (SSOTicketMessage) _sut.Parse(packet);

            // Assert
            Assert.Equal(ssoTicket, result.SSO);
        }
    }
}
