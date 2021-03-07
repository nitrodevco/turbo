using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories;
using Turbo.Database.Repositories.Player;
using Xunit;

namespace Turbo.Database.Tests.Repositories.Player
{
    public class PlayerRepositoryTests
    {
        private readonly Mock<IEmulatorContext> _turboContextMock;
        private readonly Mock<DbSet<PlayerEntity>> _dbSetMock;
        private readonly IFixture _fixture;

        private readonly IPlayerRepository _sut;

        public PlayerRepositoryTests()
        {
            _turboContextMock = new Mock<IEmulatorContext>();
            _dbSetMock = new Mock<DbSet<PlayerEntity>>();
            _turboContextMock.Setup(x => x.Players).Returns(_dbSetMock.Object);

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _sut = new PlayerRepository(_turboContextMock.Object);
        }

        [Fact]
        private async void Find_WhenPlayerExists_ReturnsPlayer()
        {
            // Arrange
            var playerId = _fixture.Create<int>();
            var player = _fixture.Build<PlayerEntity>()
                .With(x => x.Id, playerId)
                .Create();

            _dbSetMock.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(player);

            // Act
            var result = await _sut.FindAsync(playerId);

            // Assert
            _turboContextMock.Verify(x => x.Players.FindAsync(playerId), Times.Once);
            Assert.Equal(player, result);
        }
    }
}
