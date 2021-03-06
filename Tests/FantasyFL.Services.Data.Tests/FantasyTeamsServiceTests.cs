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
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fantasy;

    using MockQueryable.Moq;

    using Moq;

    using Xunit;

    public class FantasyTeamsServiceTests
    {
        [Fact]
        public async Task UserTeamIsEmptyShouldReturnTrueIfTeamExists()
        {
            var teamOne = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>
                {
                    new FantasyTeamPlayer
                    {
                    },
                },
            };

            var list = new List<FantasyTeam>();
            list.Add(teamOne);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.UserTeamIsEmpty("user1");

            Assert.False(result);
        }

        [Fact]
        public async Task UserTeamIsEmptyShouldReturnFalseIfTeamIsEmpty()
        {
            var teamOne = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
            };

            var list = new List<FantasyTeam>();
            list.Add(teamOne);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.UserTeamIsEmpty("user2");

            Assert.True(result);
        }

        [Fact]
        public async Task GetUserTeamShouldReturnCorrectTeam()
        {
            var teamOne = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
            };

            var teamTwo = new FantasyTeam
            {
                Id = "team2",
                OwnerId = "user2",
                Name = "Team 2",
            };

            var list = new List<FantasyTeam>();
            list.Add(teamOne);
            list.Add(teamTwo);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());
            var mockRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.GetUserFantasyTeam("user1");

            Assert.Equal("team1", result.Id);
            Assert.Equal("Team 1", result.Name);
        }

        [Theory]
        [InlineData("team1", 1, true, "team1", 2, true)]
        public async Task GetUserGameweekTeamShouldReturnCorrectModel(
            string firstPlayerFantasyTeamId,
            int firstPlayerId,
            bool firstPlayerIsPlaying,
            string secondPlayerFantasyTeamId,
            int secondPlayerId,
            bool secondPlayerIsPlaying)
        {
            var fantasyTeamPlayerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = firstPlayerFantasyTeamId,
                PlayerId = firstPlayerId,
                IsPlaying = firstPlayerIsPlaying,
                Player = new Player
                {
                    Id = firstPlayerId,
                    ExternId = 1,
                    Name = "Player1",
                    Age = 30,
                    Position = Position.Defender,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = secondPlayerFantasyTeamId,
                PlayerId = secondPlayerId,
                IsPlaying = secondPlayerIsPlaying,
                Player = new Player
                {
                    Id = secondPlayerId,
                    ExternId = 2,
                    Name = "Player2",
                    Age = 30,
                    Position = Position.Midfielder,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayers = new List<FantasyTeamPlayer>();
            fantasyTeamPlayers.Add(fantasyTeamPlayerOne);
            fantasyTeamPlayers.Add(fantasyTeamPlayerTwo);

            var fantasyTeamsList = new List<FantasyTeam>()
            {
                new FantasyTeam
                {
                    Id = "team1",
                    OwnerId = "user1",
                    Name = "Team 1",
                },
            };

            var playersGameweeks = new List<PlayerGameweek>();
            playersGameweeks.Add(new PlayerGameweek()
            {
                PlayerId = firstPlayerId,
                GameweekId = 1,
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
                BonusPoints = 1,
                TotalPoints = -1,
            });
            playersGameweeks.Add(new PlayerGameweek()
            {
                PlayerId = secondPlayerId,
                GameweekId = 1,
                InStartingLineup = true,
                IsSubstitute = false,
                MinutesPlayed = 90,
                Goals = 0,
                CleanSheet = true,
                YellowCards = 0,
                RedCards = 0,
                SavedPenalties = 0,
                ConcededGoals = 1,
                MissedPenalties = 0,
                OwnGoals = 0,
                BonusPoints = 1,
                TotalPoints = 3,
            });

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

            var mockFantasyTeamRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockFantasyTeamRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamsList.AsQueryable().BuildMock().Object);

            var mockFantasyTeamPlayersRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(fantasyTeamPlayers.AsQueryable().BuildMock().Object);

            var mockPlayersGameweeksRepository = fixture
                .Freeze<Mock<IRepository<PlayerGameweek>>>();

            mockPlayersGameweeksRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(playersGameweeks.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.GetUserGameweekTeam("user1");

            Assert.Equal("Team 1", result.Name);
            Assert.Equal(1, result.Gameweek);
            Assert.Equal(2, result.Players.Count());
            Assert.Equal("Player1", result.Players.First().Name);
            Assert.Equal("Defender", result.Players.First().Position);
            Assert.Equal(-1, result.Players.First().GameweekPoints);
            Assert.True(result.Players.First().IsPlaying);
        }

        [Fact]
        public async Task GetUserPlayersByPostionShouldReturnCorrectPlayers()
        {
            var fantasyTeamPlayerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 1,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 1,
                    ExternId = 1,
                    Name = "Player1",
                    Age = 30,
                    Position = Position.Defender,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 2,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 2,
                    ExternId = 2,
                    Name = "Player2",
                    Age = 30,
                    Position = Position.Defender,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayers = new List<FantasyTeamPlayer>();
            fantasyTeamPlayers.Add(fantasyTeamPlayerOne);
            fantasyTeamPlayers.Add(fantasyTeamPlayerTwo);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFantasyTeamPlayersRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamPlayers.AsQueryable().BuildMock().Object);

            var fantasyTeamsList = new List<FantasyTeam>()
            {
                new FantasyTeam
                {
                    Id = "team1",
                    OwnerId = "user1",
                    Name = "Team 1",
                },
            };

            var mockFantasyTeamRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockFantasyTeamRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamsList.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.GetUserPlayersByPosition("user1", Position.Defender);

            Assert.Equal(2, result.Count);
            Assert.Equal("Player1", result[0].Name);
            Assert.Equal("Player2", result[1].Name);
        }

        [Fact]
        public async Task GetUserTeamSelectModelShouldReturnCorrectPlayers()
        {
            var fantasyTeamPlayerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 1,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 1,
                    ExternId = 1,
                    Name = "Player1",
                    Age = 30,
                    Position = Position.Goalkeeper,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 2,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 2,
                    ExternId = 2,
                    Name = "Player2",
                    Age = 30,
                    Position = Position.Defender,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayerThree = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 3,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 3,
                    ExternId = 3,
                    Name = "Player3",
                    Age = 30,
                    Position = Position.Defender,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayerFour = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 4,
                IsPlaying = true,
                Player = new Player
                {
                    Id = 4,
                    ExternId = 4,
                    Name = "Player4",
                    Age = 30,
                    Position = Position.Attacker,
                    TeamId = 1,
                },
            };

            var fantasyTeamPlayers = new List<FantasyTeamPlayer>();
            fantasyTeamPlayers.Add(fantasyTeamPlayerOne);
            fantasyTeamPlayers.Add(fantasyTeamPlayerTwo);
            fantasyTeamPlayers.Add(fantasyTeamPlayerThree);
            fantasyTeamPlayers.Add(fantasyTeamPlayerFour);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFantasyTeamPlayersRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamPlayers.AsQueryable().BuildMock().Object);

            var fantasyTeamsList = new List<FantasyTeam>()
            {
                new FantasyTeam
                {
                    Id = "team1",
                    OwnerId = "user1",
                    Name = "Team 1",
                },
            };

            var mockFantasyTeamRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockFantasyTeamRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamsList.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = await service.GetUserTeamSelectModel("user1");

            Assert.Single(result.Goalkeepers);
            Assert.Equal(2, result.Defenders.Count);
            Assert.Empty(result.Midfielders);
            Assert.Single(result.Attackers);
        }

        [Fact]
        public async Task ClearUserPlayersShouldSetIsPlayingToFalse()
        {
            var fantasyTeamPlayerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 1,
                IsPlaying = true,
            };

            var fantasyTeamPlayerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 2,
                IsPlaying = true,
            };

            var fantasyTeamPlayers = new List<FantasyTeamPlayer>();
            fantasyTeamPlayers.Add(fantasyTeamPlayerOne);
            fantasyTeamPlayers.Add(fantasyTeamPlayerTwo);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFantasyTeamPlayersRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamPlayers.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            await service.ClearUserPlayers("team1");

            Assert.False(fantasyTeamPlayerOne.IsPlaying);
            Assert.False(fantasyTeamPlayerTwo.IsPlaying);
        }

        [Fact]
        public async Task UpdatePlayingPlayersShouldSetIsPlayingToTrue()
        {
            var fantasyTeamPlayerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 1,
                IsPlaying = false,
            };

            var fantasyTeamPlayerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 2,
                IsPlaying = false,
            };

            var fantasyTeamPlayers = new List<FantasyTeamPlayer>();
            fantasyTeamPlayers.Add(fantasyTeamPlayerOne);
            fantasyTeamPlayers.Add(fantasyTeamPlayerTwo);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFantasyTeamPlayersRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(fantasyTeamPlayers.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            await service.UpdatePlayingPlayers("team1", new HashSet<int> { 1, 2 });

            Assert.True(fantasyTeamPlayerOne.IsPlaying);
            Assert.True(fantasyTeamPlayerTwo.IsPlaying);
        }

        [Fact]
        public void GetPlayingPlayersIdsShouldReturnCorrectResult()
        {
            var playerOne = new PlayerSelectViewModel
            {
                PlayerId = 1,
                Selected = true,
            };

            var playerTwo = new PlayerSelectViewModel
            {
                PlayerId = 2,
                Selected = true,
            };

            var playerThree = new PlayerSelectViewModel
            {
                PlayerId = 3,
                Selected = true,
            };

            var playerFour = new PlayerSelectViewModel
            {
                PlayerId = 4,
                Selected = false,
            };

            var playerFive = new PlayerSelectViewModel
            {
                PlayerId = 5,
                Selected = true,
            };

            var teamModel = new TeamSelectViewModel
            {
                Goalkeepers = new List<PlayerSelectViewModel>()
                {
                    playerFive,
                },
                Defenders = new List<PlayerSelectViewModel>()
                {
                    playerOne,
                },
                Midfielders = new List<PlayerSelectViewModel>()
                {
                    playerTwo,
                },
                Attackers = new List<PlayerSelectViewModel>()
                {
                    playerThree,
                    playerFour,
                },
            };

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var service = fixture.Create<FantasyTeamsService>();

            var result = service.GetPlayingPlayersIds(teamModel);

            Assert.Equal(4, result.Count);
        }

        [Theory]
        [InlineData("team1", true)]
        [InlineData("team2", false)]
        public void FantasyTeamNameExistingWorksAsExpected(string teamName, bool expectedResult)
        {
            var fantasyTeam = new FantasyTeam
            {
                Id = "1",
                Name = "team1",
            };

            var fantasyTeams = new List<FantasyTeam>();
            fantasyTeams.Add(fantasyTeam);

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFantasyTeamRepo = fixture
                 .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();

            mockFantasyTeamRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(fantasyTeams.AsQueryable().BuildMock().Object);

            var service = fixture.Create<FantasyTeamsService>();

            var result = service.FantasyTeamNameExists(teamName);

            Assert.Equal(expectedResult, result);
        }
    }
}
