namespace FantasyFL.Services
{
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using FantasyFL.Services.Contracts;

    public class JsonDataService : IExternalDataService
    {
        public async Task<string> GetAllTeamsAsync(int leagueId, int season)
        {
            var fileName = $"teams-{leagueId}-{season}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetSquadAsync(int teamId)
        {
            var fileName = $"players-{teamId}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetRoundsJsonAsync(int league, int season)
        {
            var fileName = $"gameweeks-{season}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetFixturesByRoundAsync(string gameweekName, int season)
        {
            var fileName = $"fixtures-{gameweekName}-{season}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetLineupsJsonAsync(int fixtureId)
        {
            var fileName = $"lineups-{fixtureId}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetFixtureEventsJsonAsync(int fixtureId)
        {
            var fileName = $"events-{fixtureId}.json";
            var result = await GetResult(fileName);

            return result;
        }

        public async Task<string> GetLeaguesJsonAsync(string countryCode, int season)
        {
            var fileName = $"leagues-{countryCode}-{season}.json";
            var result = await GetResult(fileName);

            return result;
        }

        private static async Task<string> GetResult(string fileName)
        {
            var runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePath = runDir + @$"\APIFootballData\{fileName}";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            var result = await File.ReadAllTextAsync(filePath);
            return result;
        }
    }
}
