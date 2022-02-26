﻿namespace FantasyFL.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using static FantasyFL.Common.GlobalConstants;

    public class ExternalDataService : IExternalDataService
    {
        public async Task<string> GetSquadAsync(int teamId)
        {
            var url = $"https://v3.football.api-sports.io/players/squads?team={teamId}";

            string result = await this.GetResponseAsync(url);

            return result;
        }

        public async Task<string> GetAllTeamsAsync(int leagueId, int season)
        {
            var url = $"https://v3.football.api-sports.io/teams?league={leagueId}&season={season}";

            string result = await this.GetResponseAsync(url);

            return result;
        }

        public async Task<string> GetLineupsJsonAsync(int fixtureId)
        {
            var url = $"https://v3.football.api-sports.io/fixtures/lineups?fixture={fixtureId}";

            string result = await this.GetResponseAsync(url);

            return result;
        }

        public async Task<string> GetFixtureEventsJsonAsync(int fixtureId)
        {
            var url = $"https://v3.football.api-sports.io/fixtures/events?fixture={fixtureId}";

            return await this.GetResponseAsync(url);
        }

        public async Task<string> GetRoundsJsonAsync(int league, int season)
        {
            var url = $"https://v3.football.api-sports.io/fixtures/rounds?season={season}&league={league}";

            return await this.GetResponseAsync(url);
        }

        public async Task<string> GetLeaguesJsonAsync(string countryCode, int season)
        {
            var url = $"https://v3.football.api-sports.io/leagues?code={countryCode}&season={season}";

            return await this.GetResponseAsync(url);
        }

        public async Task<string> GetFixturesByRoundAsync(int round, int season)
        {
            string roundName = $"Regular Season - {round}";

            var url = $"https://v3.football.api-sports.io/fixtures?season={season}&round={roundName}&league=172";

            return await this.GetResponseAsync(url);
        }

        private async Task<string> GetResponseAsync(string url)
        {
            var apiResponseString = string.Empty;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("x-apisports-key", Key);

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                apiResponseString = await response.Content.ReadAsStringAsync();
            }

            await Task.Delay(8000);

            return apiResponseString;
        }
    }
}
