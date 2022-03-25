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
    using FantasyFL.Services.Data.Contracts;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class FixturesServiceTests
    {
        [Fact]
        public async Task GetAllInCurrentGameweekShouldWorkCorrectly()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();

            mockGameweeksService
                .Setup(x => x.GetCurrent())
                .Returns(new Gameweek
                {
                    Id = 1,
                    Name = "Gameweek 1",
                    Number = 1,
                    IsImported = true,
                    IsFinished = true,
                    EndDate = new DateTime(2022, 02, 22),
                });

            var gameFixture = new FantasyFL.Data.Models.Fixture
            {
                ExternId = 1,
                GameweekId = 1,
                HomeTeamId = 10,
                AwayTeamId = 20,
                HomeGoals = 3,
                AwayGoals = 2,
                HomeTeam = new Team
                {
                    Stadium = new Stadium(),
                },
                AwayTeam = new Team
                {
                    Stadium = new Stadium(),
                },
            };

            var list = new List<FantasyFL.Data.Models.Fixture>();
            list.Add(gameFixture);

            var mockFixturesRepo = fixture
                .Freeze<Mock<IRepository<FantasyFL.Data.Models.Fixture>>>();

            mockFixturesRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FixturesService>();

            var result = await service.GetAllInCurrentGameweek();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllInCurrentGameweekShouldWorkIfGameweekNull()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();

            mockGameweeksService
                .Setup(x => x.GetCurrent())
                .Returns((Gameweek)null);

            var service = fixture.Create<FixturesService>();

            var result = await service.GetAllInCurrentGameweek();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllInNextGameweekShouldWorkCorrectly()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();

            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns(new Gameweek
                {
                    Id = 2,
                    Name = "Gameweek 2",
                    Number = 2,
                    IsImported = true,
                    IsFinished = false,
                    EndDate = new DateTime(2022, 02, 27),
                });

            var gameFixture = new FantasyFL.Data.Models.Fixture
            {
                ExternId = 1,
                GameweekId = 2,
                HomeTeamId = 10,
                AwayTeamId = 20,
                HomeGoals = 3,
                AwayGoals = 2,
                HomeTeam = new Team
                {
                    Stadium = new Stadium(),
                },
                AwayTeam = new Team
                {
                    Stadium = new Stadium(),
                },
            };

            var list = new List<FantasyFL.Data.Models.Fixture>();
            list.Add(gameFixture);

            var mockFixturesRepo = fixture
                .Freeze<Mock<IRepository<FantasyFL.Data.Models.Fixture>>>();

            mockFixturesRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FixturesService>();

            var result = await service.GetAllInNextGameweek();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetFixturesInGameweekShouldReturnCorrectValues()
        {
            var gameFixture = new FantasyFL.Data.Models.Fixture
            {
                ExternId = 1,
                GameweekId = 1,
                HomeTeamId = 10,
                AwayTeamId = 20,
                HomeGoals = 3,
                AwayGoals = 2,
                HomeTeam = new Team
                {
                    Stadium = new Stadium(),
                },
                AwayTeam = new Team
                {
                    Stadium = new Stadium(),
                },
            };

            var list = new List<FantasyFL.Data.Models.Fixture>();
            list.Add(gameFixture);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFixturesRepo = fixture
                .Freeze<Mock<IRepository<FantasyFL.Data.Models.Fixture>>>();

            mockFixturesRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FixturesService>();

            var result = await service.GetFixturesInGameweek(1);

            Assert.Equal(3, result[0].HomeGoals);
            Assert.Equal(2, result[0].AwayGoals);
        }
    }
}
