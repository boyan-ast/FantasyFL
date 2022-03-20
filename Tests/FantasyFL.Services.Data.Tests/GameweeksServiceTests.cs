namespace FantasyFL.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoFixture;
    using AutoFixture.AutoMoq;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using Moq;
    using Xunit;

    public class GameweeksServiceTests
    {
        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectList()
        {
            var gameweekOne = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = true,
                IsFinished = true,
                EndDate = new DateTime(2022, 02, 22),
            };

            var gameweekTwo = new Gameweek
            {
                Id = 2,
                Name = "Gameweek 2",
                IsImported = false,
                IsFinished = false,
                EndDate = new DateTime(2022, 02, 28),
            };

            var list = new List<Gameweek>();
            list.Add(gameweekOne);
            list.Add(gameweekTwo);

            // Arrange
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();
            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable());
            var service = fixture.Create<GameweeksService>();

            // TODO: Make it work with ToListAsync
            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
