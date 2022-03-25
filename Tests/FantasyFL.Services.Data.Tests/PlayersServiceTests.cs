namespace FantasyFL.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AutoFixture;
    using AutoFixture.AutoMoq;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels;
    using FantasyFL.Web.ViewModels.PlayersManagement;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class PlayersServiceTests
    {
        public PlayersServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact] 
        public async Task GetAllByTeamWorks()
        {
            var playerOne = new Player
            {
                Id = 1,
                Name = "Player 1",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test",
                },
                TeamId = 1,
                Position = Position.Defender,
            };

            var playerTwo = new Player
            {
                Id = 2,
                Name = "Player 2",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test",
                },
                TeamId = 1,
                Position = Position.Attacker,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);
            playersList.Add(playerTwo);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetAllByTeam(1);

            Assert.Equal(2, result.Count);
            Assert.Equal("Player 1", result.First().Name);
            Assert.Equal(Position.Defender, result.First().Position);
            Assert.Equal("Player 2", result.Skip(1).First().Name);
            Assert.Equal(Position.Attacker, result.Skip(1).First().Position);
        }

        [Fact]
        public async Task GetAllPlayersReturnsCorrectData()
        {
            var playerOne = new Player
            {
                Id = 1,
                Name = "Player 1",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test",
                },
                TeamId = 1,
                Position = Position.Defender,
            };

            var playerTwo = new Player
            {
                Id = 2,
                Name = "Player 2",
                Team = new Team
                {
                    Id = 2,
                    Name = "Test 2",
                },
                TeamId = 2,
                Position = Position.Attacker,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);
            playersList.Add(playerTwo);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetAllPlayers();

            Assert.Equal(2, result.Count);
            Assert.Equal("Player 1", result.First().Name);
            Assert.Equal(Position.Defender, result.First().Position);
            Assert.Equal("Test", result.First().Team);
            Assert.Equal("Player 2", result.Skip(1).First().Name);
            Assert.Equal(Position.Attacker, result.Skip(1).First().Position);
            Assert.Equal("Test 2", result.Skip(1).First().Team);
        }

        [Fact]
        public async Task GetAllByPositionWorksCorrectly()
        {
            var playerOne = new Player
            {
                Id = 1,
                Name = "Player 1",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test",
                },
                TeamId = 1,
                Position = Position.Defender,
            };

            var playerTwo = new Player
            {
                Id = 2,
                Name = "Player 2",
                Team = new Team
                {
                    Id = 2,
                    Name = "Test 2",
                },
                TeamId = 2,
                Position = Position.Defender,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);
            playersList.Add(playerTwo);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetAllByPosition(Position.Defender);

            Assert.Equal(2, result.Count);
            Assert.Equal("Player 1", result.First().Name);
            Assert.Equal(Position.Defender, result.First().Position);
            Assert.Equal("Test", result.First().Team);
            Assert.Equal("Player 2", result.Skip(1).First().Name);
            Assert.Equal("Test 2", result.Skip(1).First().Team);
        }

        [Fact]
        public async Task GetPlayerTeamNameWorksCorrectly()
        {
            var playerOne = new Player
            {
                Id = 1,
                Name = "Player 1",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test Team",
                },
                TeamId = 1,
                Position = Position.Defender,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetPlayerTeamName(1);

            Assert.Equal("Test Team", result);
        }

        [Fact]
        public async Task GetPlayerIdWorksCorrectly()
        {
            var playerOne = new Player
            {
                Id = 10,
                Name = "Player 1",
                Position = Position.Defender,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetPlayerIdByName("Player 1");

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task GetPlayerGameweekPerformanceWorksCorrectly()
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

            var playersGameweeks = new List<PlayerGameweek>();
            playersGameweeks.Add(new PlayerGameweek()
            {
                Player = new Player
                {
                    Id = 1,
                    Name = "Player 1",
                    Team = new Team
                    {
                        Id = 1,
                        Name = "Team 1",
                    },
                },
                PlayerId = 1,
                GameweekId = 1,
                InStartingLineup = true,
                IsSubstitute = false,
                MinutesPlayed = 90,
                Goals = 3,
                CleanSheet = true,
                YellowCards = 0,
                RedCards = 1,
                SavedPenalties = 0,
                ConcededGoals = 2,
                MissedPenalties = 0,
                OwnGoals = 0,
                BonusPoints = 1,
                TotalPoints = 10,
            });

            var mockPlayersGameweeksRepository = fixture
                .Freeze<Mock<IRepository<PlayerGameweek>>>();

            mockPlayersGameweeksRepository
                .Setup(x => x.All())
                .Returns(playersGameweeks.AsQueryable().BuildMock().Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetPlayerGameweekPerformance(1);

            Assert.Equal(90, result.MinutesPlayed);
            Assert.Equal(3, result.Goals);
            Assert.Equal(1, result.RedCards);
            Assert.Equal(10, result.TotalPoints);
        }

        [Fact]
        public async Task GetPlayersTeamsCountReturnsCorrectResult()
        {
            var model = new PickPlayersFormModel
            {
                Goalkeepers = new List<PlayerInputModel>()
                {
                    new PlayerInputModel
                    {
                        Id = 4,
                        Name = "Player 4",
                    },
                },
                Defenders = new List<PlayerInputModel>()
                {
                    new PlayerInputModel
                    {
                        Id = 1,
                        Name = "Player 1",
                    },
                },
                Midfielders = new List<PlayerInputModel>()
                {
                    new PlayerInputModel
                    {
                        Id = 3,
                        Name = "Player 3",
                    },
                },
                Attackers = new List<PlayerInputModel>()
                {
                    new PlayerInputModel
                    {
                        Id = 2,
                        Name = "Player 2",
                    },
                },
            };

            var playerOne = new Player
            {
                Id = 1,
                Name = "Player 1",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test Team",
                },
                TeamId = 1,
                Position = Position.Defender,
            };

            var playerTwo = new Player
            {
                Id = 2,
                Name = "Player 2",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test Team",
                },
                TeamId = 1,
                Position = Position.Attacker,
            };

            var playerThree = new Player
            {
                Id = 3,
                Name = "Player 3",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test Team",
                },
                TeamId = 1,
                Position = Position.Midfielder,
            };

            var playerFour = new Player
            {
                Id = 4,
                Name = "Player 4",
                Team = new Team
                {
                    Id = 1,
                    Name = "Test Team",
                },
                TeamId = 1,
                Position = Position.Goalkeeper,
            };

            var playersList = new List<Player>();
            playersList.Add(playerOne);
            playersList.Add(playerTwo);
            playersList.Add(playerThree);
            playersList.Add(playerFour);

            var mock = playersList.AsQueryable().BuildMock();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(mock.Object);

            var service = fixture.Create<PlayersService>();

            var result = await service.GetPlayersTeamsCount(model);

            Assert.Equal(4, result["Test Team"]);
        }
    }
}