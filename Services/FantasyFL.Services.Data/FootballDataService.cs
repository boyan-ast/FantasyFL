namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.InputModels.Events;
    using FantasyFL.Services.Data.InputModels.Fixtures;
    using FantasyFL.Services.Data.InputModels.Gameweeks;
    using FantasyFL.Services.Data.InputModels.Lineups;
    using FantasyFL.Services.Data.InputModels.Players;
    using FantasyFL.Services.Data.InputModels.Teams;
    using Newtonsoft.Json;

    using static FantasyFL.Common.GlobalConstants;

    public class FootballDataService : IFootballDataService
    {
        private readonly IDictionary<string, ICollection<int>> roundsFixtures;
        private readonly IExternalDataService externalDataService;
        private readonly IDeletableEntityRepository<Team> teamsRepository;
        private readonly IDeletableEntityRepository<Player> playersRepository;

        public FootballDataService(
            IExternalDataService externalDataService,
            IDeletableEntityRepository<Team> teamsRepository,
            IDeletableEntityRepository<Player> playersRepository)
        {
            this.roundsFixtures = new Dictionary<string, ICollection<int>>();
            this.externalDataService = externalDataService;
            this.teamsRepository = teamsRepository;
            this.playersRepository = playersRepository;
        }

        public async Task<IEnumerable<TeamStadiumDto>> GetTeamsAndStadiumsJsonAsync(int leagueId, int season)
        {
            var json = await this.externalDataService.GetAllTeamsAsync(leagueId, season);

            var teamsResponse = JsonConvert.DeserializeObject<TeamsResponseDto>(json);

            if (teamsResponse.Results == 0)
            {
                throw new ArgumentException($"League {leagueId} in season {season} does not contain any teams.");
            }

            return teamsResponse.Response;
        }

        public async Task<TeamPlayersInfoDto> GetTeamSquadJsonAsync(int teamId)
        {
            var json = await this.externalDataService.GetSquadAsync(teamId);

            var squadResponse = JsonConvert.DeserializeObject<PlayersResponseDto>(json);

            if (squadResponse.Results == 0)
            {
                throw new ArgumentException($"Team {teamId} does not exist.");
            }

            return squadResponse.Response[0];
        }

        public async Task<string[]> GetAllRoundsAsync(int leagueId, int seasonId)
        {
            var roundsJson = await this.externalDataService.GetRoundsJsonAsync(leagueId, seasonId);

            var roundsResponse = JsonConvert.DeserializeObject<GameweeksResponseDto>(roundsJson);

            if (roundsResponse.Results == 0)
            {
                throw new ArgumentException($"No gameweeks in league {leagueId} for season {seasonId}.");
            }

            return roundsResponse.Rounds;
        }

        public async Task<IEnumerable<FixtureInfoDto>> GetAllFixturesByGameweekAsync(int gameweek, int year = SeasonYear)
        {
            var fixturesJson = await this.externalDataService.GetFixturesByRoundAsync(gameweek, year);

            var fixturesResponse = JsonConvert.DeserializeObject<FixturesResponseDto>(fixturesJson);

            if (fixturesResponse.Results == 0)
            {
                throw new ArgumentException($"No fixtures in Gameweek {gameweek} for year {year}.");
            }

            return fixturesResponse.FixturesInfo;
        }

        public async Task<IEnumerable<TeamLineupDto>> GetLineupsAsync(int fixtureId)
        {
            var lineupsJson = await this.externalDataService.GetLineupsJsonAsync(fixtureId);

            lineupsJson = lineupsJson.Replace(@"""id"":null,", @"""id"":9999999,");

            var lineups = JsonConvert.DeserializeObject<LineupsResponseDto>(lineupsJson);

            if (lineups.Results != 2)
            {
                throw new ArgumentException("Lineups have to be 2.");
            }

            return lineups.Response;
        }

        public async Task<IEnumerable<EventDto>> GetFixtureEventsAsync(int fixtureId)
        {
            var fixtureJson = await this.externalDataService.GetFixtureEventsJsonAsync(fixtureId);

            var fixtureInfo = JsonConvert.DeserializeObject<EventResponseDto>(fixtureJson);

            return fixtureInfo.Response;
        }

        public ICollection<int> GetFixturesIds(string roundName)
        {
            return this.roundsFixtures[roundName];
        }

        public async Task SetTeamsTopPlayers()
        {
            var topPlayersNumbers = new Dictionary<string, int>
            {
                { "Ludogorets", 11 },
                { "Botev Plovdiv", 8 },
                { "Levski Sofia", 7 },
                { "Cherno More Varna", 72 },
                { "CSKA Sofia", 2 },
                { "Slavia Sofia", 10 },
                { "Beroe", 30 },
                { "Lokomotiv Plovdiv", 14 },
                { "Botev Vratsa", 18 },
                { "Tsarsko Selo", 3 },
                { "Lokomotiv Sofia", 58 },
                { "CSKA 1948", 18 },
                { "Pirin Blagoevgrad", 29 },
                { "Arda Kardzhali", 17 },
            };

            var teams = this.teamsRepository.All().ToList();
            var players = this.playersRepository.All().ToList();

            foreach (var team in teams)
            {
                var player = this.playersRepository
                    .All()
                    .Where(p => p.TeamId == team.Id)
                    .FirstOrDefault(p => p.Number == topPlayersNumbers[team.Name]);

                team.TopPlayer = player;
            }

            await this.teamsRepository.SaveChangesAsync();
        }
    }
}
