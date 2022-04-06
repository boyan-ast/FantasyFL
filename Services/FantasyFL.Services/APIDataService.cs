namespace FantasyFL.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using FantasyFL.Services.Contracts;
    using Microsoft.Extensions.Configuration;

    using static FantasyFL.Common.GlobalConstants;

    public class APIDataService : IExternalDataService
    {
        private readonly IConfiguration config;
        private readonly string apiKey;

        public APIDataService(IConfiguration config)
        {
            this.config = config;
            this.apiKey = this.config["APIFootballKey"];
        }

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

        public async Task<string> GetFixturesByRoundAsync(string gameweekName, int season)
        {
            var url = $"https://v3.football.api-sports.io/fixtures?season={season}&round={gameweekName}&league={LeagueExternId}";

            return await this.GetResponseAsync(url);
        }

        private async Task<string> GetResponseAsync(string url)
        {
            var apiResponseString = string.Empty;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("x-apisports-key", this.apiKey);

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
