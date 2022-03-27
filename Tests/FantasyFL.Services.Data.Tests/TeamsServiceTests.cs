namespace FantasyFL.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class TeamsServiceTests
    {
        public TeamsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllReturnsCorrectViewModels()
        {
            var teamOne = new Team
            {
                Id = 1,
                Name = "Test Team",
                Logo = "logoOne.jpg",
                Stadium = new Stadium
                {
                    Name = "Test Stadium 1",
                },
            };

            var teamTwo = new Team
            {
                Id = 2,
                Name = "Test Team Two",
                Logo = "logoTwo.jpg",
                Stadium = new Stadium
                {
                    Name = "Test Stadium 2",
                },
            };

            var teams = new List<Team>();
            teams.Add(teamOne);
            teams.Add(teamTwo);

            var mockRepo = new Mock<IDeletableEntityRepository<Team>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(teams.AsQueryable().BuildMock().Object);

            var service = new TeamsService(mockRepo.Object);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Test Team", result.First().Name);
            Assert.Equal("Test Team Two", result.Skip(1).First().Name);
            Assert.Equal("logoOne.jpg", result.First().Logo);
            Assert.Equal("logoTwo.jpg", result.Skip(1).First().Logo);
            Assert.Equal(1, result.First().Id);
            Assert.Equal(2, result.Skip(1).First().Id);
            Assert.Equal("Test Stadium 1", result.First().StadiumName);
            Assert.Equal("Test Stadium 2", result.Skip(1).First().StadiumName);
        }

        [Fact]
        public async Task GetTeamPlayersReturnsCorrectViewModels()
        {
            var team = new Team
            {
                Id = 1,
                Name = "Test Team",
                Logo = "logoOne.jpg",
                Stadium = new Stadium
                {
                    Name = "Test Stadium 1",
                },
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = 101,
                        Name = "Player 1",
                        Team = new Team
                        {
                            Name = "Test Team",
                        },
                        Position = Position.Defender,
                    },
                    new Player
                    {
                        Id = 102,
                        Name = "Player 2",
                        Team = new Team
                        {
                            Name = "Test Team",
                        },
                        Position = Position.Midfielder,
                    },
                },
            };

            var teams = new List<Team>();
            teams.Add(team);

            var mockRepo = new Mock<IDeletableEntityRepository<Team>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(teams.AsQueryable().BuildMock().Object);

            var service = new TeamsService(mockRepo.Object);

            var result = await service.GetTeamPlayers(1);

            Assert.Equal("Test Team", result.Name);
            Assert.Equal("logoOne.jpg", result.Logo);
            Assert.Equal("Test Stadium 1", result.StadiumName);
            Assert.Equal(2, result.Players.Count());
        }
    }
}
