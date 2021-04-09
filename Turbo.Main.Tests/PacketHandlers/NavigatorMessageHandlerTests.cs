using AutoFixture;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Revisions;
using Turbo.Main.PacketHandlers;
using Turbo.Networking.Game.Clients;
using Turbo.Packets;
using Turbo.Packets.Incoming.Navigator;
using Turbo.Players;
using Xunit;

namespace Turbo.Main.Tests.PacketHandlers
{
    public class NavigatorMessageHandlerTests
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly Mock<INavigatorManager> _navigatorManagerMock;

        private readonly ISession _session;
        private readonly IPlayer _player;

        private readonly IFixture _fixture;

        private readonly INavigatorMessageHandler _sut;

        public NavigatorMessageHandlerTests()
        {
            // Hub
            _messageHub = new PacketMessageHub(new NullLogger<PacketMessageHub>());
            _navigatorManagerMock = new Mock<INavigatorManager>();

            // Session
            var channelHandlerContextMock = new Mock<IChannelHandlerContext>();
            var revisionMock = new Mock<IRevision>();
            _session = new Session(channelHandlerContextMock.Object, revisionMock.Object, new NullLogger<Session>());

            // Player
            var playerManagerMock = new Mock<IPlayerManager>();
            _player = new Player(new NullLogger<IPlayer>(), playerManagerMock.Object, new Database.Entities.Players.PlayerEntity());

            // Object mocking object
            _fixture = new Fixture();

            // System under test
            _sut = new NavigatorMessageHandler(_messageHub, _navigatorManagerMock.Object);
        }

        [Fact]
        private void Constructor_SubscribesToMessages()
        {
            Assert.True(_messageHub.Exists<GetGuestRoomMessage>(_sut));
            Assert.True(_messageHub.Exists<NewNavigatorInitMessage>(_sut));
        }

        [Fact]
        private void OnNewNavigatorInit_WithPlayer_SendsPackets()
        {
            // Arrange
            _session.SetPlayer(_player);

            // Act
            _messageHub.Publish(new NewNavigatorInitMessage(), _session);

            // Assert
            _navigatorManagerMock.Verify(x => x.SendNavigatorMetaData(_session.Player), Times.Once);
            _navigatorManagerMock.Verify(x => x.SendNavigatorLiftedRooms(_session.Player), Times.Once);
            _navigatorManagerMock.Verify(x => x.SendNavigatorSavedSearches(_session.Player), Times.Once);
            _navigatorManagerMock.Verify(x => x.SendNavigatorEventCategories(_session.Player), Times.Once);
        }

        [Fact]
        private void OnNewNavigatorInit_WithoutPlayer_DoesNotSendPackets()
        {
            // Act
            _messageHub.Publish(new NewNavigatorInitMessage(), _session);

            // Assert
            _navigatorManagerMock.Verify(x => x.SendNavigatorMetaData(_session.Player), Times.Never);
            _navigatorManagerMock.Verify(x => x.SendNavigatorLiftedRooms(_session.Player), Times.Never);
            _navigatorManagerMock.Verify(x => x.SendNavigatorSavedSearches(_session.Player), Times.Never);
            _navigatorManagerMock.Verify(x => x.SendNavigatorEventCategories(_session.Player), Times.Never);
        }

        [Fact]
        private void GetGuestRoomMessage_WithPlayer_SendsPackets()
        {
            // Arrange
            _session.SetPlayer(_player);
            var message = _fixture.Create<GetGuestRoomMessage>();

            // Act
            _messageHub.Publish(message, _session);

            // Assert
            _navigatorManagerMock.Verify(x => x.GetGuestRoomMessage(_player, message.RoomId, message.EnterRoom, message.RoomForward), Times.Once);
        }

        [Fact]
        private void GetGuestRoomMessage_WithoutPlayer_DoesNotSendPackets()
        {
            // Act
            _messageHub.Publish(new NewNavigatorInitMessage(), _session);

            // Assert
            _navigatorManagerMock.Verify(x => x.GetGuestRoomMessage(It.IsAny<IPlayer>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
