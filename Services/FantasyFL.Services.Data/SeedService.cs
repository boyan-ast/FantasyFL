namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Common;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Contracts;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Data.InputModels.Teams;

    using static FantasyFL.Common.GlobalConstants;

    public class SeedService : ISeedService
    {
        private readonly IFootballDataService footballDataService;
        private readonly IParseService parseService;
        private readonly IRepository<Gameweek> gameweeksRepository;
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<Team> teamsRepository;
        private readonly IRepository<Fixture> fixturesRepository;

        public SeedService(
            IFootballDataService footballDataService,
            IParseService parseService,
            IRepository<Gameweek> gameweeksRepository,
            IDeletableEntityRepository<Player> playersRepository,
            IDeletableEntityRepository<Team> teamsRepository,
            IRepository<Stadium> stadiumsRepository,
            IRepository<Fixture> fixturesRepository)
        {
            this.footballDataService = footballDataService;
            this.parseService = parseService;
            this.gameweeksRepository = gameweeksRepository;
            this.playersRepository = playersRepository;
            this.teamsRepository = teamsRepository;
            this.fixturesRepository = fixturesRepository;
        }

        public IEnumerable<TeamStadiumDto> TeamsAndStadiumsDto { get; private set; } = new List<TeamStadiumDto>();

        public async Task ImportGameweeks()
        {
            var gameweeks = await this.footballDataService.GetAllRoundsAsync(LeagueExternId, SeasonYear);

            foreach (var gameweek in gameweeks)
            {
                var newGameweek = new Gameweek
                {
                    Name = gameweek,
                    Number = int.Parse(gameweek.Split(" - ")[1]),
                    EndDate = this.parseService.ParseDate(CustomData.GameweeksEndDates[gameweek], "dd.MM.yyyy"),
                };

                await this.gameweeksRepository.AddAsync(newGameweek);
            }

            await this.gameweeksRepository.SaveChangesAsync();
        }

        public async Task ImportFixtures()
        {
            var gameweeks = this
                .gameweeksRepository
                .All()
                .OrderBy(gw => gw.Number)
                .Select(gw => new
                {
                    gw.Id,
                    gw.Name,
                });

            foreach (var gameweek in gameweeks)
            {
                var fixturesInfo = await this.footballDataService
                    .GetAllFixturesByGameweekAsync(gameweek.Name, SeasonYear);

                foreach (var fixtureDto in fixturesInfo)
                {
                    var externId = fixtureDto.Fixture.Id;

                    var fixtureDate = this.parseService
                        .ParseDate(fixtureDto.Fixture.Date.Split("T")[0], "yyyy-MM-dd");

                    var homeTeamId = this.teamsRepository
                        .All()
                        .FirstOrDefault(t => t.ExternId == fixtureDto.Teams.HomeTeam.Id)
                        .Id;

                    var awayTeamId = this.teamsRepository
                        .All()
                        .FirstOrDefault(t => t.ExternId == fixtureDto.Teams.AwayTeam.Id)
                        .Id;

                    var status = fixtureDto.Fixture.Status.Status;
                    var homeGoals = fixtureDto.Goals.HomeGoals;
                    var awayGoals = fixtureDto.Goals.AwayGoals;

                    var newFixture = new Fixture
                    {
                        ExternId = externId,
                        GameweekId = gameweek.Id,
                        Date = fixtureDate,
                        HomeTeamId = homeTeamId,
                        AwayTeamId = awayTeamId,
                        Status = status,
                        HomeGoals = homeGoals,
                        AwayGoals = awayGoals,
                    };

                    await this.fixturesRepository.AddAsync(newFixture);
                }
            }

            await this.fixturesRepository.SaveChangesAsync();
        }

        public async Task ImportPlayers()
        {
            var teamIds = this.teamsRepository
                .All()
                .Select(t => new
                {
                    Id = t.Id,
                    ExternId = t.ExternId,
                })
                .ToList();

            foreach (var team in teamIds)
            {
                var squadDto = await this.footballDataService.GetTeamSquadJsonAsync(team.ExternId);

                foreach (var player in squadDto.Players)
                {
                    var newPlayer = new Player
                    {
                        ExternId = player.Id,
                        Name = player.Name,
                        Age = player.Age,
                        Number = player.Number,
                        Position = Enum.Parse<Position>(player.Position, true),
                        TeamId = team.Id,
                    };

                    await this.playersRepository.AddAsync(newPlayer);
                }
            }

            await this.playersRepository.SaveChangesAsync();
        }

        public async Task ImportTeams()
        {
            if (!this.TeamsAndStadiumsDto.Any())
            {
                this.TeamsAndStadiumsDto =
                    (await this.footballDataService.GetTeamsAndStadiumsJsonAsync(LeagueExternId, SeasonYear))
                    .ToList();
            }

            foreach (var teamDto in this.TeamsAndStadiumsDto)
            {
                var team = new Team
                {
                    ExternId = teamDto.Team.Id,
                    Name = teamDto.Team.Name,
                    Logo = teamDto.Team.Logo.Split("teams/")[1],
                    Stadium = new Stadium
                    {
                        ExternId = teamDto.Stadium.Id,
                        Name = teamDto.Stadium.Name,
                        City = teamDto.Stadium.City,
                        Capacity = teamDto.Stadium.Capacity,
                        Image = teamDto.Stadium.Image,
                    },
                };

                await this.teamsRepository.AddAsync(team);
            }

            await this.teamsRepository.SaveChangesAsync();
        }
    }
}
