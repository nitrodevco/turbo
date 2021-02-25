using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Rooms.Utils;
using Xunit;

namespace Turbo.Rooms.Tests.Utils
{
    public class PointTests
    {
        private readonly IFixture _fixture;

        public PointTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        private void Clone_ReturnsCopyOfPoint()
        {
            // Arrange
            var x = _fixture.Create<int>();
            var y = _fixture.Create<int>();
            var z = _fixture.Create<int>();

            IPoint point = new Point(x, y, z);

            // Act
            var clonedPoint = point.Clone();

            // Assert
            Assert.False(ReferenceEquals(point, clonedPoint));
            Assert.Equal(point.X, clonedPoint.X);
            Assert.Equal(point.Y, clonedPoint.Y);
            Assert.Equal(point.Z, clonedPoint.Z);
        }

        [Fact]
        private void AddPoint_AddsValuesToPoint()
        {
            // Arrange
            var x = _fixture.Create<int>();
            var y = _fixture.Create<int>();
            var z = _fixture.Create<int>();

            var addX = _fixture.Create<int>();
            var addY = _fixture.Create<int>();
            var addZ = _fixture.Create<int>();

            IPoint point = new Point(x, y, z);
            IPoint addPoint = new Point(addX, addY, addZ);

            // Act
            var addedPoint = point.AddPoint(addPoint);

            // Assert
            Assert.False(ReferenceEquals(point, addedPoint));
            Assert.False(ReferenceEquals(addPoint, addedPoint));
            Assert.Equal(point.X + addPoint.X, addedPoint.X);
            Assert.Equal(point.Y + addPoint.Y, addedPoint.Y);
            Assert.Equal(point.Z + addPoint.Z, addedPoint.Z);
        }

        [Fact]
        private void SubtractPoint_SubtractsValuesFromPoint()
        {
            // Arrange
            var x = _fixture.Create<int>();
            var y = _fixture.Create<int>();
            var z = _fixture.Create<int>();

            var subtractX = _fixture.Create<int>();
            var subtractY = _fixture.Create<int>();
            var subtractZ = _fixture.Create<int>();

            IPoint point = new Point(x, y, z);
            IPoint subtractPoint = new Point(subtractX, subtractY, subtractZ);

            // Act
            var subtractedPoint = point.SubtractPoint(subtractPoint);

            // Assert
            Assert.False(ReferenceEquals(point, subtractedPoint));
            Assert.False(ReferenceEquals(subtractPoint, subtractedPoint));
            Assert.Equal(point.X - subtractPoint.X, subtractedPoint.X);
            Assert.Equal(point.Y - subtractPoint.Y, subtractedPoint.Y);
            Assert.Equal(point.Z - subtractPoint.Z, subtractedPoint.Z);
        }

        [Fact]
        private void AdjustPoint_AdjustsValuesFromPoint()
        {
            // Arrange
            var x = _fixture.Create<int>();
            var y = -_fixture.Create<int>();
            var z = _fixture.Create<int>();

            var adjustX = _fixture.Create<int>();
            var adjustY = _fixture.Create<int>();
            var adjustZ = _fixture.Create<int>();

            IPoint point = new Point(x, y, z);
            IPoint adjustPoint = new Point(adjustX, adjustY, adjustZ);

            // Act
            var adjustedPoint = point.AdjustPoint(adjustPoint);

            // Assert
            Assert.False(ReferenceEquals(point, adjustedPoint));
            Assert.False(ReferenceEquals(adjustPoint, adjustedPoint));
            Assert.Equal(point.X + adjustPoint.X, adjustedPoint.X);
            Assert.Equal(point.Y + adjustPoint.Y, adjustedPoint.Y);
            Assert.Equal(point.Z + adjustPoint.Z, adjustedPoint.Z);
        }
    }
}
