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
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class TransfersServiceTests
    {
        public TransfersServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        public static IEnumerable<object[]> PlayersData =>
            new List<object[]>
            {
                new object[]
                {
                    new Player
                    {
                        Id = 1,
                        Name = "Removed Player",
                        Position = Position.Goalkeeper,
                        TeamId = 1001,
                        Team = new Team
                        {
                            Name = "UserTeam",
                        },
                    },
                    new Player
                    {
                        Id = 101,
                        Name = "Player One",
                        Position = Position.Goalkeeper,
                        TeamId = 2001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 201,
                        Name = "Player Two",
                        Position = Position.Defender,
                        TeamId = 2001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 301,
                        Name = "Player Three",
                        Position = Position.Attacker,
                        TeamId = 2001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 401,
                        Name = "Player Four",
                        Position = Position.Goalkeeper,
                        TeamId = 3001,
                        Team = new Team
                        {
                            Name = "Team Two",
                        },
                    },
                    2,
                },
                new object[]
                {
                    new Player
                    {
                        Id = 1,
                        Name = "Removed Player",
                        Position = Position.Goalkeeper,
                        TeamId = 1001,
                        Team = new Team
                        {
                            Name = "UserTeam",
                        },
                    },
                    new Player
                    {
                        Id = 101,
                        Name = "Player One",
                        Position = Position.Goalkeeper,
                        TeamId = 2001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 201,
                        Name = "Player Two",
                        Position = Position.Goalkeeper,
                        TeamId = 3001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 301,
                        Name = "Player Three",
                        Position = Position.Goalkeeper,
                        TeamId = 4001,
                        Team = new Team
                        {
                            Name = "Team One",
                        },
                    },
                    new Player
                    {
                        Id = 401,
                        Name = "Player Four",
                        Position = Position.Goalkeeper,
                        TeamId = 4001,
                        Team = new Team
                        {
                            Name = "Team Two",
                        },
                    },
                    5,
                },
            };

        [Fact]
        public async Task AddPlayerAddsCorrectDataToTheDatabase()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>(),
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            var service = fixture.Create<TransfersService>();

            await service.AddPlayer("user1", 1);

            Assert.Single(userTeam.FantasyTeamPlayers);
            Assert.Equal(1, userTeam.FantasyTeamPlayers.First().PlayerId);
        }

        [Theory]
        [MemberData(nameof(PlayersData))]
        public async Task GetPlayersToTransferReturnsCorrectlyFilteredPlayers(
            Player removed,
            Player first,
            Player second,
            Player third,
            Player fourth,
            int expectedCount)
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var removedPlayer = removed;

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                    new FantasyTeamPlayer
                    {
                        Player = removedPlayer,
                    },
                    new FantasyTeamPlayer
                    {
                        Player = first,
                    },
                    new FantasyTeamPlayer
                    {
                        Player = second,
                    },
                    new FantasyTeamPlayer
                    {
                        Player = third,
                    },
                    new FantasyTeamPlayer
                    {
                        Player = fourth,
                    },
                },
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            var mockFantasyTeamPlayersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<Player>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.AllAsNoTracking())
                .Returns(new List<Player> { removedPlayer, first, second, third, fourth }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var service = fixture.Create<TransfersService>();

            var result = await service.GetPlayersToTransfer("user1", Position.Goalkeeper);

            Assert.Equal(expectedCount, result.Count);
        }

        [Theory]
        [MemberData(nameof(PlayersData))]
        public async Task GetTransfersListReturnCorrectData(
            Player first,
            Player second,
            Player third,
            Player fourth,
            Player fifth,
            int count)
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();
            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns(new Gameweek { Id = 10, });

            var userFantasyTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "Team 1",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                    new FantasyTeamPlayer() { Player = first, },
                    new FantasyTeamPlayer() { Player = second, },
                    new FantasyTeamPlayer() { Player = third, },
                    new FantasyTeamPlayer() { Player = fourth, },
                    new FantasyTeamPlayer() { Player = fifth, },
                },
            };

            var userGameweek = new ApplicationUserGameweek()
            {
                UserId = "user1",
                GameweekId = 10,
                Gameweek = new Gameweek { Number = 10, },
                Transfers = 1,
                User = new ApplicationUser
                {
                    FantasyTeam = userFantasyTeam,
                },
            };

            var mockUsersGameweeksRepository = fixture
                .Freeze<Mock<IRepository<ApplicationUserGameweek>>>();
            mockUsersGameweeksRepository
                .Setup(x => x.All())
                .Returns(new List<ApplicationUserGameweek> { userGameweek }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var service = fixture.Create<TransfersService>();

            var result = await service.GetTransfersList("user1");

            if (count != 5)
            {
                count = 5;
            }

            Assert.Equal(count, result.Players.Count());
        }

        [Fact]
        public async Task GetTransfersListThrowsIfGameweekNull()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();
            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns((Gameweek)null);

            var service = fixture.Create<TransfersService>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetTransfersList("user1"));
        }

        [Fact]
        public async Task RemovePlayerWorksProperly()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var fantasyTeamPlayerToRemove = new FantasyTeamPlayer
            {
                Player = new Player
                {
                    Id = 101,
                },
            };

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "UserTeam",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                   fantasyTeamPlayerToRemove,
                },
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            var mockFantasyTeamPlayersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(new List<FantasyTeamPlayer>
                    {
                        new FantasyTeamPlayer { FantasyTeamId = "team1", PlayerId = 101, },
                    }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();
            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns(new Gameweek { Id = 10, });

            var userGameweek = new ApplicationUserGameweek
            {
                UserId = "user1",
                GameweekId = 10,
                Transfers = 1,
            };

            var mockUserGameweekRepo = fixture
                .Freeze<Mock<IRepository<ApplicationUserGameweek>>>();
            mockUserGameweekRepo
                .Setup(x => x.All())
                .Returns(new List<ApplicationUserGameweek>() { userGameweek, }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var service = fixture.Create<TransfersService>();

            await service.RemovePlayer("user1", 101);

            Assert.Equal(0, userGameweek.Transfers);
        }

        [Fact]
        public async Task RemovePlayerThrowsIfGameweekNull()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var fantasyTeamPlayerToRemove = new FantasyTeamPlayer
            {
                Player = new Player
                {
                    Id = 101,
                },
            };

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "UserTeam",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                   fantasyTeamPlayerToRemove,
                },
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            var mockFantasyTeamPlayersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(new List<FantasyTeamPlayer>
                    {
                        new FantasyTeamPlayer { FantasyTeamId = "team1", PlayerId = 101, },
                    }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();
            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns((Gameweek)null);

            var service = fixture.Create<TransfersService>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayer("user1", 1));
        }

        [Fact]
        public async Task RemovePlayerThrowsIfZeroTransfersLeft()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var fantasyTeamPlayerToRemove = new FantasyTeamPlayer
            {
                Player = new Player
                {
                    Id = 101,
                },
            };

            var userTeam = new FantasyTeam
            {
                Id = "team1",
                OwnerId = "user1",
                Name = "UserTeam",
                FantasyTeamPlayers = new List<FantasyTeamPlayer>()
                {
                   fantasyTeamPlayerToRemove,
                },
            };

            var mockUsersService = fixture
                .Freeze<Mock<IUsersService>>();
            mockUsersService
                .Setup(x => x.GetUserFantasyTeam(It.IsAny<string>()))
                .Returns(Task.FromResult(userTeam));

            var mockFantasyTeamPlayersRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<FantasyTeamPlayer>>>();

            mockFantasyTeamPlayersRepo
                .Setup(x => x.All())
                .Returns(new List<FantasyTeamPlayer>
                    {
                        new FantasyTeamPlayer { FantasyTeamId = "team1", PlayerId = 101, },
                    }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var mockGameweeksService = fixture
                .Freeze<Mock<IGameweeksService>>();
            mockGameweeksService
                .Setup(x => x.GetNext())
                .Returns(new Gameweek { Id = 10, });

            var userGameweek = new ApplicationUserGameweek
            {
                UserId = "user1",
                GameweekId = 10,
                Transfers = 0,
            };

            var mockUserGameweekRepo = fixture
                .Freeze<Mock<IRepository<ApplicationUserGameweek>>>();
            mockUserGameweekRepo
                .Setup(x => x.All())
                .Returns(new List<ApplicationUserGameweek>() { userGameweek, }
                    .AsQueryable()
                    .BuildMock()
                    .Object);

            var service = fixture.Create<TransfersService>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayer("user1", 1));
        }
    }
}
