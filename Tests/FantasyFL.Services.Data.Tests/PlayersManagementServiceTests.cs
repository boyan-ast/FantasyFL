namespace FantasyFL.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.PlayersManagement;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class PlayersManagementServiceTests
    {
        [Fact]
        public async Task GetUserTeamReturnsCorrectResult()
        {
            var team = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Test Team",
            };

            var list = new List<FantasyTeam>();
            list.Add(team);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyTeam>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new PlayersManagementService(
                mockRepo.Object,
                new Mock<IDeletableEntityRepository<FantasyTeamPlayer>>().Object);

            var result = await service.GetUserTeam("user1");

            Assert.Equal("team1", result.Id);
            Assert.Equal("Test Team", result.Name);
        }

        [Fact]
        public async Task GetFantasyTeamPlayersReturnCorrectListOfPlayers()
        {
            var team = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Test Team",
            };

            var teamsList = new List<FantasyTeam>();
            teamsList.Add(team);

            var playerOne = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 1,
            };

            var playerTwo = new FantasyTeamPlayer
            {
                FantasyTeamId = "team1",
                PlayerId = 2,
            };

            var playersList = new List<FantasyTeamPlayer>();
            playersList.Add(playerOne);
            playersList.Add(playerTwo);

            var mockFantasyTeamsRepo = new Mock<IDeletableEntityRepository<FantasyTeam>>();

            mockFantasyTeamsRepo
                .Setup(x => x.All())
                .Returns(teamsList.AsQueryable().BuildMock().Object);

            var mockFantasyTeamsPlayersRepo = new Mock<IDeletableEntityRepository<FantasyTeamPlayer>>();

            mockFantasyTeamsPlayersRepo
                .Setup(x => x.All())
                .Returns(playersList.AsQueryable().BuildMock().Object);

            var service = new PlayersManagementService(
                mockFantasyTeamsRepo.Object,
                mockFantasyTeamsPlayersRepo.Object);

            var result = await service.GetFantasyTeamPlayers("user1");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddPlayersWorksProperly()
        {
            var team = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Test Team",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>(),
            };

            var list = new List<FantasyTeam>();
            list.Add(team);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyTeam>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new PlayersManagementService(
                mockRepo.Object,
                new Mock<IDeletableEntityRepository<FantasyTeamPlayer>>().Object);

            await service.AddPlayersToTeam(
                new PickPlayersFormModel()
                {
                    Goalkeepers = new List<PlayerInputModel>()
                    {
                        new PlayerInputModel()
                        {
                            Id = 1,
                        },
                    },
                    Defenders = new List<PlayerInputModel>()
                    {
                        new PlayerInputModel()
                        {
                            Id = 2,
                        },
                    },
                    Midfielders = new List<PlayerInputModel>()
                    {
                        new PlayerInputModel()
                        {
                            Id = 3,
                        },
                    },
                    Attackers = new List<PlayerInputModel>()
                    {
                        new PlayerInputModel()
                        {
                            Id = 4,
                        },
                    },
                },
                "user1");

            Assert.Equal(4, team.FantasyTeamPlayers.Count);
            Assert.Contains(1, team.FantasyTeamPlayers.Select(x => x.PlayerId));
            Assert.Contains(2, team.FantasyTeamPlayers.Select(x => x.PlayerId));
            Assert.Contains(3, team.FantasyTeamPlayers.Select(x => x.PlayerId));
            Assert.Contains(4, team.FantasyTeamPlayers.Select(x => x.PlayerId));
            Assert.DoesNotContain(5, team.FantasyTeamPlayers.Select(x => x.PlayerId));
        }
    }
}
