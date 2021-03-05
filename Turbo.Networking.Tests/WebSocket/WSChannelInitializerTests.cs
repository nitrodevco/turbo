using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.Game.WebSocket.Codec;
using Xunit;

namespace Turbo.Networking.Tests.WebSocket
{
    public class WSChannelInitializerTests
    {
        private readonly Mock<IChannel> _channelMock;
        private readonly Mock<IChannelPipeline> _channelPipelineMock;
        private readonly WSChannelInitializerMock _sut;

        public WSChannelInitializerTests()
        {
            _channelMock = new Mock<IChannel>();
            _channelPipelineMock = new Mock<IChannelPipeline>();
            _channelMock.SetupGet(x => x.Pipeline).Returns(_channelPipelineMock.Object);
            _sut = new WSChannelInitializerMock();
        }

        [Fact]
        private void Initialize_InitializesChannel()
        {
            // Arrange
            _channelPipelineMock.Setup(x => x.AddLast(It.IsAny<string>(), It.IsAny<IChannelHandler>())).Returns(_channelPipelineMock.Object);

            // Act
            _sut.ExecuteInitChannel(_channelMock.Object);

            // Assert
            Assert.True(_sut is WSChannelInitializer);
            _channelPipelineMock.Verify(x => x.AddLast("httpCodec", It.IsAny<HttpServerCodec>()));
            _channelPipelineMock.Verify(x => x.AddLast("objectAggregator", It.Is<HttpObjectAggregator>(h => h.MaxContentLength.Equals(65536))));
            _channelPipelineMock.Verify(x => x.AddLast("wsProtocolHandler", It.IsAny<WebSocketServerProtocolHandler>()));
            _channelPipelineMock.Verify(x => x.AddLast("websocketCodec", It.IsAny<WebSocketCodec>()));
            _channelPipelineMock.Verify(x => x.AddLast("frameEncoder", It.IsAny<FrameLengthFieldEncoder>()));
            _channelPipelineMock.Verify(x => x.AddLast("frameDecoder", It.IsAny<FrameLengthFieldDecoder>()));
            _channelPipelineMock.Verify(x => x.AddLast("gameEncoder", It.IsAny<GameEncoder>()));
            _channelPipelineMock.Verify(x => x.AddLast("gameDecoder", It.IsAny<GameDecoder>()));
            _channelPipelineMock.Verify(x => x.AddLast("messageHandler", It.IsAny<GameMessageHandler>()));
            _channelPipelineMock.Verify(x => x.AddLast(It.IsAny<string>(), It.IsAny<IChannelHandler>()), Times.Exactly(9));
        }
    }

    internal class WSChannelInitializerMock : WSChannelInitializer
    {
        public WSChannelInitializerMock() : base() { }

        public void ExecuteInitChannel(IChannel channel)
        {
            base.InitChannel(channel);
        }
    }
}
