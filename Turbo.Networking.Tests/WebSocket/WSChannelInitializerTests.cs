using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using Moq;
using System;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.Game.WebSocket.Codec;
using Turbo.Packets;
using Turbo.Packets.Revisions;
using Xunit;

namespace Turbo.Networking.Tests.WebSocket
{
    public class WSChannelInitializerTests
    {
        private readonly Mock<IChannel> _channelMock;
        private readonly Mock<IChannelPipeline> _channelPipelineMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        private readonly Mock<IPacketMessageHub> _packetMessageHubMock;
        private readonly Mock<ISessionManager> _sessionManagerMock;
        private readonly Mock<IRevisionManager> _revisionManagerMock;
        private readonly Mock<ISessionFactory> _sessionFactoryMock;

        private readonly WSChannelInitializerMock _sut;

        public WSChannelInitializerTests()
        {
            _channelMock = new Mock<IChannel>();
            _channelPipelineMock = new Mock<IChannelPipeline>();
            _channelMock.SetupGet(x => x.Pipeline).Returns(_channelPipelineMock.Object);

            _packetMessageHubMock = new Mock<IPacketMessageHub>();
            _sessionManagerMock = new Mock<ISessionManager>();
            _revisionManagerMock = new Mock<IRevisionManager>();
            _sessionFactoryMock = new Mock<ISessionFactory>();

            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceProviderMock.Setup(x => x.GetService(typeof(IPacketMessageHub))).Returns(_packetMessageHubMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(ISessionManager))).Returns(_sessionManagerMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IRevisionManager))).Returns(_revisionManagerMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(ISessionFactory))).Returns(_sessionFactoryMock.Object);

            _sut = new WSChannelInitializerMock(_serviceProviderMock.Object);
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
        public WSChannelInitializerMock(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public void ExecuteInitChannel(IChannel channel)
        {
            base.InitChannel(channel);
        }
    }
}
