using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Turbo.Database.Repositories;
using Xunit;

namespace Turbo.Database.Tests.Repositories
{
    public class HabboRepositoryTests
    {
        private readonly Mock<IEmulatorContext> _turboContextMock;
        private readonly Mock<DbSet<Habbo>> _dbSetMock;
        private readonly IFixture _fixture;

        private readonly IHabboRepository _sut;

        public HabboRepositoryTests()
        {
            _turboContextMock = new Mock<IEmulatorContext>();
            _dbSetMock = new Mock<DbSet<Habbo>>();
            _turboContextMock.Setup(x => x.Habbos).Returns(_dbSetMock.Object);
            _fixture = new Fixture();

            _sut = new HabboRepository(_turboContextMock.Object);
        }

        [Fact]
        private async void Find_WhenHabboExists_ReturnsHabbo()
        {
            // Arrange
            var habboId = _fixture.Create<int>();
            var habbo = _fixture.Build<Habbo>()
                .With(x => x.Id, habboId)
                .Create();

            _dbSetMock.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(habbo);

            // Act
            var result = await _sut.FindAsync(habboId);

            // Assert
            _turboContextMock.Verify(x => x.Habbos.FindAsync(habboId), Times.Once);
            Assert.Equal(habbo, result);
        }
    }
}
