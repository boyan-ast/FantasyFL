namespace FantasyFL.Services.Data.Tests
{
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
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        public UsersServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetUserFantasyTeamReturnsCorrectTeam()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                    new FantasyTeamPlayer { Player = new Player() },
                    new FantasyTeamPlayer { Player = new Player() },
                    new FantasyTeamPlayer { Player = new Player() },
                },
            };

            var mockFantasyTeamRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();
            mockFantasyTeamRepo
                .Setup(x => x.All())
                .Returns(new List<FantasyTeam> { userTeam }.AsQueryable().BuildMock().Object);

            var service = fixture.Create<UsersService>();

            var result = await service.GetUserFantasyTeam("user1");

            Assert.Equal("team1", result.Id);
            Assert.Equal("Team 1", result.Name);
            Assert.Equal(3, result.FantasyTeamPlayers.Count);
        }

        [Fact]
        public async Task GetUserTeamViewModelReturnsCorrectModel()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Owner = new ApplicationUser { TotalPoints = 11, },
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                    new FantasyTeamPlayer
                    {
                        PlayerId = 1,
                        Player = new Player()
                        {
                            Name = "Player 1",
                            Position = Position.Defender,
                            Team = new Team
                            {
                                Name = "Team 1",
                            },
                        },
                    },
                    new FantasyTeamPlayer
                    {
                        PlayerId = 2,
                        Player = new Player()
                        {
                            Name = "Player 2",
                            Position = Position.Attacker,
                            Team = new Team
                            {
                                Name = "Team 2",
                            },
                        },
                    },
                },
            };

            var mockFantasyTeamRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeam>>>();
            mockFantasyTeamRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(new List<FantasyTeam> { userTeam }.AsQueryable().BuildMock().Object);

            var service = fixture.Create<UsersService>();

            var result = await service.GetUserTeamViewModel("user1");

            Assert.Equal("Team 1", result.Name);
            Assert.Equal(11, result.TotalPoints);
            Assert.Equal(2, result.FantasyTeamPlayers.Count());
        }

        [Fact]
        public async Task AddingUserFutureGameweeksWorks()
        {
            var fixture = new AutoFixture.Fixture()
               .Customize(new AutoMoqCustomization());

            var mockGameweeksRepo = fixture
                .Freeze<Mock<IRepository<Gameweek>>>();

            var firstGameweek = new Gameweek
            {
                Id = 1,
                Number = 1,
            };

            var secondGameweek = new Gameweek
            {
                Id = 2,
                Number = 2,
            };

            var gameweeksList = new List<Gameweek>()
            {
                firstGameweek,
                secondGameweek,
            };

            mockGameweeksRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(gameweeksList.AsQueryable().BuildMock().Object);

            var mockUsersGameweeksRepo = fixture
                .Freeze<Mock<IRepository<ApplicationUserGameweek>>>();

            var userGameweeks = new List<ApplicationUserGameweek>();

            mockUsersGameweeksRepo
                .Setup(x => x.AddAsync(It.IsAny<ApplicationUserGameweek>()))
                .Callback((ApplicationUserGameweek aug) => userGameweeks.Add(aug));

            var service = fixture.Create<UsersService>();

            await service.AddUserGameweeks("user1", 1);

            Assert.Equal(2, userGameweeks.Count);
        }

        [Fact]
        public void GetUserLeaguesReturnsCorrectModel()
        {
            var fixture = new AutoFixture.Fixture()
               .Customize(new AutoMoqCustomization());

            var league = new FantasyLeague
            {
                Id = 101,
                Name = "Test League",
                ApplicationUsers = new List<ApplicationUser>()
                {
                    new ApplicationUser { Id = "user1", },
                    new ApplicationUser { Id = "user2", },
                    new ApplicationUser { Id = "user3", },
                },
            };

            var mockFantasyLeaguesRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyLeague>>>();

            mockFantasyLeaguesRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(new List<FantasyLeague>() { league, }.AsQueryable().BuildMock().Object);

            var service = fixture.Create<UsersService>();

            var result = service.GetUserLeagues("user1");

            Assert.Single(result);
            Assert.Equal(3, result.First().ParticipantsIds.Count());
            Assert.Equal("Test League", result.First().Name);
        }

        [Fact]
        public async Task GetUserByIdReturnsUser()
        {
            var fixture = new AutoFixture.Fixture()
               .Customize(new AutoMoqCustomization());

            var mockUsersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<ApplicationUser>>>();

            var userOne = new ApplicationUser
            {
                Id = "user1",
                UserName = "The User",
            };

            var userTwo = new ApplicationUser
            {
                Id = "user2",
            };

            var users = new List<ApplicationUser>();
            users.Add(userOne);
            users.Add(userTwo);

            mockUsersRepo
                .Setup(x => x.All())
                .Returns(users.AsQueryable().BuildMock().Object);

            var service = fixture.Create<UsersService>();

            var result = await service.GetUserById("user1");

            Assert.Equal("The User", result.UserName);
        }
    }
}
