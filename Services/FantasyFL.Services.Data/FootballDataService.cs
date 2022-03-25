namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        private readonly IExternalDataService externalDataService;

        public FootballDataService(IExternalDataService externalDataService)
        {
            this.externalDataService = externalDataService;
        }

        public async Task<IEnumerable<TeamStadiumDto>> GetTeamsAndStadiumsJsonAsync(int leagueId, int season)
        {
            var json = await this.externalDataService.GetAllTeamsAsync(leagueId, season);

            var teamsResponse = JsonConvert.DeserializeObject<TeamsResponseDto>(json);

            return teamsResponse.Response;
        }

        public async Task<TeamPlayersInfoDto> GetTeamSquadJsonAsync(int teamId)
        {
            var json = await this.externalDataService.GetSquadAsync(teamId);

            var squadResponse = JsonConvert.DeserializeObject<PlayersResponseDto>(json);

            return squadResponse.Response[0];
        }

        public async Task<string[]> GetAllRoundsAsync(int leagueId, int seasonId)
        {
            var roundsJson = await this.externalDataService.GetRoundsJsonAsync(leagueId, seasonId);

            var roundsResponse = JsonConvert.DeserializeObject<GameweeksResponseDto>(roundsJson);

            return roundsResponse.Rounds;
        }

        public async Task<IEnumerable<FixtureInfoDto>> GetAllFixturesByGameweekAsync(string gameweekName, int year = SeasonYear)
        {
            var fixturesJson = await this.externalDataService.GetFixturesByRoundAsync(gameweekName, year);

            var fixturesResponse = JsonConvert.DeserializeObject<FixturesResponseDto>(fixturesJson);

            return fixturesResponse.FixturesInfo;
        }

        public async Task<IEnumerable<TeamLineupDto>> GetLineupsAsync(int fixtureId)
        {
            var lineupsJson = await this.externalDataService.GetLineupsJsonAsync(fixtureId);

            lineupsJson = lineupsJson.Replace(@"""id"":null,", @"""id"":9999999,");

            var lineups = JsonConvert.DeserializeObject<LineupsResponseDto>(lineupsJson);

            return lineups.Response;
        }

        public async Task<IEnumerable<EventDto>> GetFixtureEventsAsync(int fixtureId)
        {
            var fixtureJson = await this.externalDataService.GetFixtureEventsJsonAsync(fixtureId);

            var fixtureInfo = JsonConvert.DeserializeObject<EventResponseDto>(fixtureJson);

            return fixtureInfo.Response;
        }
    }
}
