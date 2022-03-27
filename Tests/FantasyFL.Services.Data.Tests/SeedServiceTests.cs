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
    using FantasyFL.Services.Contracts;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Data.InputModels.Fixtures;
    using FantasyFL.Services.Data.InputModels.Players;
    using FantasyFL.Services.Data.InputModels.Teams;
    using Moq;
    using Xunit;

    public class SeedServiceTests
    {
        [Fact]
        public async Task GetAllByTeamWorks()
        {
            var list = new List<Gameweek>();

            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockFootballDataService = fixture.Freeze<Mock<IFootballDataService>>();
            mockFootballDataService
                .Setup(x => x.GetAllRoundsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new string[] { "Regular Season - 1" }));

            var mockParseDateService = fixture.Freeze<Mock<IParseService>>();
            mockParseDateService
                .Setup(x => x.ParseDate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new DateTime(2022, 3, 25));

            var mockGameweeksRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();
            mockGameweeksRepo
                .Setup(x => x.AddAsync(It.IsAny<Gameweek>()))
                .Callback((Gameweek gw) => list.Add(gw));

            var service = fixture.Create<SeedService>();

            await service.ImportGameweeks();

            Assert.Equal("Regular Season - 1", list[0].Name);
            Assert.Equal(1, list[0].Number);
            Assert.Equal(new DateTime(2022, 3, 25), list[0].EndDate);
            Assert.True(list[0].IsImported);
            Assert.True(list[0].IsFinished);
        }

        [Fact]
        public async Task ImportFixturesAddsCorrectly()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockParseDateService = fixture.Freeze<Mock<IParseService>>();
            mockParseDateService
                .Setup(x => x.ParseDate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new DateTime(2022, 3, 25));

            var gameweeks = new List<Gameweek>();
            gameweeks.Add(new Gameweek
            {
                Id = 1,
                Name = "Regular Season - 1",
            });

            var mockGameweeksRepo = fixture.Freeze<Mock<IRepository<Gameweek>>>();
            mockGameweeksRepo
                .Setup(x => x.All())
                .Returns(gameweeks.AsQueryable());

            var fixturesInfoDto = new List<FixtureInfoDto>();
            fixturesInfoDto.Add(
                new FixtureInfoDto()
                {
                    Fixture = new FixtureDto()
                    {
                        Id = 1,
                        Date = "2022-03-25",
                        Status = new FixtureStatusDto()
                        {
                            Status = "FT",
                        },
                    },
                    Teams = new FixtureTeamsDto()
                    {
                        HomeTeam = new FixtureTeamInfoDto()
                        {
                            Id = 100,
                        },
                        AwayTeam = new FixtureTeamInfoDto()
                        {
                            Id = 200,
                        },
                    },
                    Goals = new FixtureGoalsDto()
                    {
                        HomeGoals = 2,
                        AwayGoals = 3,
                    },
                });

            var mockFootballDataService = fixture.Freeze<Mock<IFootballDataService>>();
            mockFootballDataService
                .Setup(x => x.GetAllFixturesByGameweekAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<FixtureInfoDto>>(fixturesInfoDto));

            var mockTeamsRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Team>>>();
            mockTeamsRepo
                .Setup(x => x.All())
                .Returns(new List<Team>()
                    {
                        new Team
                        {
                            Id = 10,
                            ExternId = 100,
                        },
                        new Team
                        {
                            Id = 20,
                            ExternId = 200,
                        },
                    }
                    .AsQueryable());

            var fixtures = new List<FantasyFL.Data.Models.Fixture>();

            var mockFixturesRepo = fixture
                .Freeze<Mock<IRepository<FantasyFL.Data.Models.Fixture>>>();
            mockFixturesRepo
                .Setup(x => x.AddAsync(It.IsAny<FantasyFL.Data.Models.Fixture>()))
                .Callback((FantasyFL.Data.Models.Fixture fix) => fixtures.Add(fix));

            var service = fixture.Create<SeedService>();

            await service.ImportFixtures();

            Assert.Single(fixtures);
            Assert.Equal(1, fixtures.First().ExternId);
            Assert.Equal(1, fixtures.First().GameweekId);
            Assert.Equal(new DateTime(2022, 3, 25), fixtures.First().Date);
            Assert.Equal(10, fixtures.First().HomeTeamId);
            Assert.Equal(20, fixtures.First().AwayTeamId);
            Assert.Equal(2, fixtures.First().HomeGoals);
            Assert.Equal(3, fixtures.First().AwayGoals);
            Assert.Equal("FT", fixtures.First().Status);
        }

        [Fact]
        public async Task ImportPlayersWorksAsExpected()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockTeamsRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Team>>>();
            mockTeamsRepo
                .Setup(x => x.All())
                .Returns(new List<Team>()
                    {
                        new Team
                        {
                            Id = 10,
                            ExternId = 100,
                        },
                    }
                    .AsQueryable());

            var teamPlayersInfo = new TeamPlayersInfoDto()
            {
                Players = new PlayerInfoDto[]
                {
                    new PlayerInfoDto
                    {
                        Id = 101,
                        Name = "Test Player",
                        Age = 18,
                        Number = 9,
                        Position = "Attacker",
                    },
                },
            };

            var mockFootballDataService = fixture.Freeze<Mock<IFootballDataService>>();
            mockFootballDataService
                .Setup(x => x.GetTeamSquadJsonAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(teamPlayersInfo));

            var players = new List<Player>();

            var mockFixturesRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<Player>>>();
            mockFixturesRepo
                .Setup(x => x.AddAsync(It.IsAny<Player>()))
                .Callback((Player player) => players.Add(player));

            var service = fixture.Create<SeedService>();

            await service.ImportPlayers();

            Assert.Single(players);
            Assert.Equal(101, players.First().ExternId);
            Assert.Equal("Test Player", players.First().Name);
            Assert.Equal(18, players.First().Age);
            Assert.Equal(Position.Attacker, players.First().Position);
            Assert.Equal(10, players.First().TeamId);
        }

        [Fact]
        public async Task ImportTeamsWorksAsExpected()
        {
            var fixture = new AutoFixture.Fixture()
                .Customize(new AutoMoqCustomization());

            var mockTeamsRepo = fixture.Freeze<Mock<IDeletableEntityRepository<Team>>>();
            mockTeamsRepo
                .Setup(x => x.All())
                .Returns(new List<Team>()
                    {
                        new Team
                        {
                            Id = 10,
                            ExternId = 100,
                        },
                    }
                    .AsQueryable());

            var teamInfo = new TeamStadiumDto()
            {
                Team = new TeamInfoDto
                {
                    Id = 301,
                    Name = "Test Team",
                    Logo = "something/teams/logo.jpg",
                },
                Stadium = new StadiumInfoDto
                {
                    Id = 1001,
                    Name = "Test Stadium",
                    City = "City of Testers",
                    Capacity = 101,
                    Image = "no.image",
                },
            };
            var teamsDto = new List<TeamStadiumDto>();
            teamsDto.Add(teamInfo);

            var mockFootballDataService = fixture.Freeze<Mock<IFootballDataService>>();
            mockFootballDataService
                .Setup(x => x.GetTeamsAndStadiumsJsonAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<TeamStadiumDto>>(teamsDto));

            var teams = new List<Team>();

            var mockFixturesRepo = fixture
                .Freeze<Mock<IDeletableEntityRepository<Team>>>();
            mockFixturesRepo
                .Setup(x => x.AddAsync(It.IsAny<Team>()))
                .Callback((Team team) => teams.Add(team));

            var service = fixture.Create<SeedService>();

            await service.ImportTeams();

            Assert.Single(teams);
            Assert.Equal(301, teams.First().ExternId);
            Assert.Equal("Test Team", teams.First().Name);
            Assert.Equal("logo.jpg", teams.First().Logo);
            Assert.Equal(1001, teams.First().Stadium.ExternId);
            Assert.Equal("Test Stadium", teams.First().Stadium.Name);
            Assert.Equal("City of Testers", teams.First().Stadium.City);
            Assert.Equal(101, teams.First().Stadium.Capacity);
            Assert.Equal("no.image", teams.First().Stadium.Image);
        }
    }
}
