namespace FantasyFL.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels;
    using FantasyFL.Web.ViewModels.Leagues;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class LeaguesServiceTests
    {
        public LeaguesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetLeagueByNameShouldWorkProperly()
        {
            var league = new FantasyLeague
            {
                Id = 1,
                Name = "TestLeague",
            };

            var list = new List<FantasyLeague>();
            list.Add(league);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new LeaguesService(
                mockRepo.Object,
                new Mock<IUsersService>().Object);

            var result = await service.GetLeagueByName("TestLeague");

            Assert.Equal(1, result.Id);
            Assert.Equal("TestLeague", result.Name);
        }

        [Fact]
        public async Task GetLeagueByNameReturnsNullIfNotExistingLeagueNameProvided()
        {
            var list = new List<FantasyLeague>();

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new LeaguesService(
                mockRepo.Object,
                new Mock<IUsersService>().Object);

            var result = await service.GetLeagueByName("TestLeague");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetStandingsWorksProperly()
        {
            var league = new FantasyLeague
            {
                Id = 1,
                Name = "TestLeague",
                ApplicationUsers = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        Id = "user1",
                        UserName = "username1",
                        TotalPoints = 10,
                        FantasyTeam = new FantasyTeam
                        {
                            Id = "team1",
                            Name = "TestTeam1",
                        },
                    },
                    new ApplicationUser
                    {
                        Id = "user2",
                        UserName = "username2",
                        TotalPoints = 3,
                        FantasyTeam = new FantasyTeam
                        {
                            Id = "team2",
                            Name = "TestTeam2",
                        },
                    },
                },
            };

            var list = new List<FantasyLeague>();
            list.Add(league);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new LeaguesService(
                mockRepo.Object,
                new Mock<IUsersService>().Object);

            var result = await service.GetLeagueStandings(1);

            Assert.Equal("TestLeague", result.Name);
            Assert.Equal(2, result.Participants);
            Assert.Equal(2, result.ApplicationUsers.Count());
        }

        [Fact]
        public async Task GetAllReturnsCorrectResult()
        {
            var league = new FantasyLeague
            {
                Id = 1,
                Name = "TestLeague",
                ApplicationUsers = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        Id = "user1",
                    },
                    new ApplicationUser
                    {
                        Id = "user2",
                    },
                },
            };

            var leagueTwo = new FantasyLeague
            {
                Id = 2,
                Name = "TestLeague2",
            };

            var list = new List<FantasyLeague>();
            list.Add(league);
            list.Add(leagueTwo);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable().BuildMock().Object);

            var service = new LeaguesService(
                mockRepo.Object,
                new Mock<IUsersService>().Object);

            var result = await service.GetAllLeagues();

            Assert.Equal(2, result.Count);
            Assert.Equal("TestLeague", result[0].Name);
            Assert.Contains("user1", result[0].ParticipantsIds);
            Assert.Contains("user2", result[0].ParticipantsIds);
        }

        [Fact]
        public async Task JoinLeaguesAddsUserToLeague()
        {
            var league = new FantasyLeague
            {
                Id = 1,
                Name = "TestLeague",
                ApplicationUsers = new List<ApplicationUser>(),
            };

            var list = new List<FantasyLeague>();
            list.Add(league);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(x => x.GetUserById("user1"))
                .Returns(Task.FromResult(new ApplicationUser
                {
                    Id = "user1",
                    UserName = "TestUser",
                }));

            var service = new LeaguesService(
                mockRepo.Object,
                mockUsersService.Object);

            await service.JoinLeague(1, "user1");

            Assert.Single(league.ApplicationUsers);
            Assert.Contains("user1", league.ApplicationUsers.Select(u => u.Id));
            Assert.Contains("TestUser", league.ApplicationUsers.Select(u => u.UserName));
        }

        [Fact]
        public async Task JoinLeaguesThrowsIfUserAlereadyIsInTheLeague()
        {
            var league = new FantasyLeague
            {
                Id = 1,
                Name = "TestLeague",
                ApplicationUsers = new List<ApplicationUser>()
                {
                    new ApplicationUser
                    {
                        Id = "user1",
                        UserName = "TestUser",
                    },
                },
            };

            var list = new List<FantasyLeague>();
            list.Add(league);

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.All())
                .Returns(list.AsQueryable().BuildMock().Object);

            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(x => x.GetUserById("user1"))
                .Returns(Task.FromResult(new ApplicationUser
                {
                    Id = "user1",
                    UserName = "TestUser",
                }));

            var service = new LeaguesService(
                mockRepo.Object,
                mockUsersService.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.JoinLeague(1, "user1"));
        }

        [Fact]
        public async Task CreateLeagueWorksProperly()
        {
            var list = new List<FantasyLeague>();

            var mockRepo = new Mock<IDeletableEntityRepository<FantasyLeague>>();

            mockRepo
                .Setup(x => x.AddAsync(It.IsAny<FantasyLeague>()))
                .Callback((FantasyLeague league) => list.Add(league));

            mockRepo
               .Setup(x => x.All())
               .Returns(list.AsQueryable().BuildMock().Object);

            var mockUsersService = new Mock<IUsersService>();
            mockUsersService
                .Setup(x => x.GetUserById("user1"))
                .Returns(Task.FromResult(new ApplicationUser
                {
                    Id = "user1",
                    UserName = "TestUser",
                }));

            var service = new LeaguesService(
                mockRepo.Object,
                mockUsersService.Object);

            var leagueInputModel = new CreateLeagueInputModel
            {
                Name = "TestLeague",
                Description = "TestTestTest",
            };

            await service.CreateLeague(leagueInputModel, "user1");

            Assert.Equal("TestLeague", list[0].Name);
            Assert.Equal("TestTestTest", list[0].Description);
            Assert.Contains("user1", list[0].ApplicationUsers.Select(u => u.Id));
            Assert.Contains("TestUser", list[0].ApplicationUsers.Select(u => u.UserName));
        }
    }
}
