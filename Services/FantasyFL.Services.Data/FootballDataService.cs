namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Contracts;
    using FantasyFL.Services.Data.Contracts;
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

        public async Task<IEnumerable<FixtureInfoDto>> GetAllFixturesByGameweekAsync(string gameweekName, int year = SeasonYear)
        {
            var fixturesJson = await this.externalDataService.GetFixturesByRoundAsync(gameweekName, year);

            var fixturesResponse = JsonConvert.DeserializeObject<FixturesResponseDto>(fixturesJson);

            if (fixturesResponse.Results == 0)
            {
                throw new ArgumentException($"No fixtures in {gameweekName} for year {year}.");
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
    }
}
