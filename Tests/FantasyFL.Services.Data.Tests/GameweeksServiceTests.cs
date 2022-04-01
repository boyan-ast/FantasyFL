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
    using MockQueryable.Moq;
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

            var mock = list.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);
            var service = fixture.Create<GameweeksService>();

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Gameweek 1", result[0].Name);
            Assert.True(result[0].IsImported);
            Assert.True(result[0].IsFinished);
            Assert.Equal(new DateTime(2022, 02, 22), result[0].EndDate);
            Assert.True(result[0].PreviousIsFinished);
            Assert.True(result[1].PreviousIsFinished);
        }

        [Fact]
        public void GetCurrentShouldReturnCorrectGameweek()
        {
            var gameweek = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = true,
                IsFinished = true,
                EndDate = new DateTime(2022, 02, 22),
            };

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(new List<Gameweek>() { gameweek }.AsQueryable());
            var service = fixture.Create<GameweeksService>();

            var result = service.GetCurrent();

            Assert.Equal("Gameweek 1", result.Name);
            Assert.True(result.IsImported);
            Assert.True(result.IsFinished);
            Assert.Equal(new DateTime(2022, 02, 22), result.EndDate);
        }

        [Fact]
        public void GetCurrentShouldThrowIfNull()
        {
            var gameweek = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = false,
                IsFinished = false,
                EndDate = new DateTime(2022, 02, 22),
            };

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(new List<Gameweek>() { gameweek }.AsQueryable());
            var service = fixture.Create<GameweeksService>();

            Assert.Throws<InvalidOperationException>(() => service.GetCurrent());
        }

        [Fact]
        public void GetNextShouldReturnCorrectGameweek()
        {
            var gameweek = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = true,
                IsFinished = false,
                EndDate = new DateTime(2022, 02, 22),
            };

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(new List<Gameweek>() { gameweek }.AsQueryable());
            var service = fixture.Create<GameweeksService>();

            var result = service.GetNext();

            Assert.Equal("Gameweek 1", result.Name);
            Assert.True(result.IsImported);
            Assert.False(result.IsFinished);
            Assert.Equal(new DateTime(2022, 02, 22), result.EndDate);
        }

        [Fact]
        public void GetNextShouldThrowIfNull()
        {
            var gameweek = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = true,
                IsFinished = true,
                EndDate = new DateTime(2022, 02, 22),
            };

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(new List<Gameweek>() { gameweek }.AsQueryable());
            var service = fixture.Create<GameweeksService>();

            Assert.Throws<InvalidOperationException>(() => service.GetNext());
        }

        [Theory]
        [InlineData(1, 1, true, false, 90, 1, true, 0, 0, 0, 1, 0, 0, 10, true)]
        [InlineData(2, 1, true, false, 90, 1, true, 0, 0, 0, 1, 0, 0, 7, true)]
        [InlineData(3, 1, false, false, 0, 1, true, 0, 0, 0, 1, 0, 0, 2, false)]
        public async Task CalculateUserGameweekPointsShouldReturnCorrectResult(
            int playerId,
            int gameweekId,
            bool inStartingLineup,
            bool isSubstitute,
            int minutesPlayed,
            int goals,
            bool cleanSheet,
            int yellowCards,
            int redCards,
            int savedPenalties,
            int concededGoals,
            int missedPenalties,
            int ownGoals,
            int totalPoints,
            bool playerIsPlaying)
        {
            var playerGameweek = new PlayerGameweek
            {
                PlayerId = playerId,
                GameweekId = gameweekId,
                InStartingLineup = inStartingLineup,
                IsSubstitute = isSubstitute,
                MinutesPlayed = minutesPlayed,
                Goals = goals,
                CleanSheet = cleanSheet,
                YellowCards = yellowCards,
                RedCards = redCards,
                SavedPenalties = savedPenalties,
                ConcededGoals = concededGoals,
                MissedPenalties = missedPenalties,
                OwnGoals = ownGoals,
                TotalPoints = totalPoints,
            };

            var list = new List<PlayerGameweek>();
            list.Add(playerGameweek);

            var mockPlayerGameweek = list.AsQueryable().BuildMock();

            var mockFantasyTeam = new List<FantasyTeam>()
            {
                new FantasyTeam
                {
                    Id = "team",
                    OwnerId = "user",
                    Name = "Test",
                    IsDeleted = false,
                },
            }.AsQueryable().BuildMock();

            var mockFantasyTeamPlayers = new List<FantasyTeamPlayer>()
            {
                new FantasyTeamPlayer
                {
                    FantasyTeamId = "team",
                    PlayerId = playerId,
                    IsPlaying = playerIsPlaying,
                    IsDeleted = false,
                },
            }.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepoPlayerGameweek = fixture.Freeze<Mock<IRepository<PlayerGameweek>>>();
            var mockRepoFantasyTeam = fixture.Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();
            var mockRepoFantasyTeamsPlayers = fixture.Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockRepoFantasyTeam
                .Setup(x => x.All())
                .Returns(mockFantasyTeam.Object);

            mockRepoFantasyTeamsPlayers
                .Setup(x => x.AllAsNoTracking())
                .Returns(mockFantasyTeamPlayers.Object);

            mockRepoPlayerGameweek
                .Setup(x => x.AllAsNoTracking())
                .Returns(mockPlayerGameweek.Object);

            var service = fixture.Create<GameweeksService>();

            var result = await service.CalculateUserGameweekPoints("user", 1);

            var expected = playerIsPlaying ? totalPoints : 0;

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task FinishGameweekShouldThrowIfNotImported()
        {
            var gameweek = new Gameweek
            {
                Id = 1,
                Name = "Gameweek 1",
                IsImported = false,
                IsFinished = false,
                EndDate = new DateTime(2022, 02, 22),
            };

            var list = new List<Gameweek>();
            list.Add(gameweek);

            var mock = list.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);
            var service = fixture.Create<GameweeksService>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.FinishGameweek(1));
        }

        [Fact]
        public async Task FinishGameweekShouldWorkCorrectly()
        {
            var gameweek = new Gameweek
            {
                Id = 2,
                Name = "Gameweek 2",
                Number = 2,
                IsImported = true,
                IsFinished = false,
                EndDate = new DateTime(2022, 02, 22),
            };

            var list = new List<Gameweek>();
            list.Add(gameweek);

            var mock = list.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var mockUsersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<ApplicationUser>>>();

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "user1",
                    StartGameweek = new Gameweek
                    {
                        Id = 1,
                        Number = 1,
                    },
                },
            };

            mockUsersRepo
                .Setup(x => x.All())
                .Returns(users.AsQueryable().BuildMock().Object);

            var mockUsersGameweeksRepo = fixture
                .Freeze<Mock<IRepository<ApplicationUserGameweek>>>();

            var usersGameweeks = new List<ApplicationUserGameweek>()
            {
                new ApplicationUserGameweek
                {
                    UserId = "user1",
                    GameweekId = 2,
                },
            };

            mockUsersGameweeksRepo
                .Setup(x => x.All())
                .Returns(usersGameweeks.AsQueryable().BuildMock().Object);

            var playerGameweek = new PlayerGameweek
            {
                PlayerId = 1,
                GameweekId = 2,
                InStartingLineup = true,
                IsSubstitute = false,
                MinutesPlayed = 90,
                Goals = 0,
                CleanSheet = true,
                YellowCards = 0,
                RedCards = 0,
                SavedPenalties = 0,
                ConcededGoals = 2,
                MissedPenalties = 0,
                OwnGoals = 0,
                TotalPoints = 3,
            };

            var playersGameweeksList = new List<PlayerGameweek>();
            playersGameweeksList.Add(playerGameweek);

            var mockPlayerGameweekRepo = playersGameweeksList.AsQueryable().BuildMock();

            var mockFantasyTeam = new List<FantasyTeam>()
            {
                new FantasyTeam
                {
                    Id = "team1",
                    OwnerId = "user1",
                    Name = "Test Team",
                },
            }.AsQueryable().BuildMock();

            var mockFantasyTeamPlayers = new List<FantasyTeamPlayer>()
            {
                new FantasyTeamPlayer
                {
                    FantasyTeamId = "team1",
                    PlayerId = 1,
                    IsPlaying = true,
                    IsDeleted = false,
                },
            }.AsQueryable().BuildMock();

            var mockRepoPlayerGameweek = fixture.Freeze<Mock<IRepository<PlayerGameweek>>>();
            var mockRepoFantasyTeam = fixture.Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();
            var mockRepoFantasyTeamsPlayers = fixture.Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockRepoFantasyTeam
                .Setup(x => x.All())
                .Returns(mockFantasyTeam.Object);

            mockRepoFantasyTeamsPlayers
                .Setup(x => x.AllAsNoTracking())
                .Returns(mockFantasyTeamPlayers.Object);

            mockRepoPlayerGameweek
                .Setup(x => x.AllAsNoTracking())
                .Returns(mockPlayerGameweekRepo.Object);

            var service = fixture.Create<GameweeksService>();

            await service.FinishGameweek(2);

            Assert.Equal(3, users.First().TotalPoints);
        }

        [Fact]
        public async Task GetNumberByIdReturnsCorrectResult()
        {
            var gameweek = new Gameweek
            {
                Id = 21,
                Name = "Gameweek 21",
                Number = 23,
            };

            var list = new List<Gameweek>();
            list.Add(gameweek);

            var mock = list.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();

            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(mock.Object);
            var service = fixture.Create<GameweeksService>();

            var result = await service.GetGameweekNumberById(21);

            Assert.Equal(23, result);
        }
    }
}
