//using DotNetty.Transport.Channels;
//using Microsoft.Extensions.Logging.Abstractions;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Turbo.Core.Game.Navigator;
//using Turbo.Core.Game.Players;
//using Turbo.Core.Networking.Game.Clients;
//using Turbo.Core.PacketHandlers;
//using Turbo.Core.Packets;
//using Turbo.Core.Packets.Messages;
//using Turbo.Core.Packets.Revisions;
//using Turbo.Main.PacketHandlers;
//using Turbo.Networking.Game.Clients;
//using Turbo.Packets;
//using Turbo.Packets.Incoming.Navigator;
//using Turbo.Players;
//using Xunit;

//namespace Turbo.Main.Tests.PacketHandlers
//{
//    public class NavigatorMessageHandlerTests
//    {
//        private readonly Mock<IPacketMessageHub> _messageHubMock;
//        private readonly Mock<INavigatorManager> _navigatorManagerMock;

//        private readonly ISession _session;
//        private readonly IPlayer _player;

//        private readonly INavigatorMessageHandler _sut;

//        public NavigatorMessageHandlerTests()
//        {
//            _messageHubMock = new Mock<IPacketMessageHub>();
//            _navigatorManagerMock = new Mock<INavigatorManager>();

//            var channelHandlerContextMock = new Mock<IChannelHandlerContext>();
//            var revisionMock = new Mock<IRevision>();
//            _session = new Session(channelHandlerContextMock.Object, revisionMock.Object, new NullLogger<Session>());

//            var playerContainerMock = new Mock<IPlayerContainer>();
//            _player = new Player(playerContainerMock.Object, new NullLogger<IPlayer>(), new Database.Entities.Players.PlayerEntity());

//            _sut = new NavigatorMessageHandler(_messageHubMock.Object, _navigatorManagerMock.Object);
//        }

//        [Fact]
//        private void Constructor_SubscribesToMessages()
//        {
//            _messageHubMock.Verify(x => x.Subscribe<GetGuestRoomMessage>(_sut, _sut.))
//        }

//        [Fact]
//        private void OnNewNavigatorInit_WithPlayer_SendsPackets()
//        {
//            // Arrange
//            _session.SetPlayer(_player);

//            // Act
//            _messageHubMock.Publish(new NewNavigatorInitMessage(), _session);

//            // Assert
//            _navigatorManagerMock.Verify(x => x.SendNavigatorMetaData(_session.Player), Times.Once);
//            _navigatorManagerMock.Verify(x => x.SendNavigatorLiftedRooms(_session.Player), Times.Once);
//        }

//        [Fact]
//        private void OnNewNavigatorInit_WithoutPlayer_SendsPackets()
//        {
//            // Act
//            _messageHub.Publish(new NewNavigatorInitMessage(), _session);

//            // Assert
//            _navigatorManagerMock.Verify(x => x.SendNavigatorMetaData(_session.Player), Times.Never);
//            _navigatorManagerMock.Verify(x => x.SendNavigatorLiftedRooms(_session.Player), Times.Never);
//        }
//    }
//}
