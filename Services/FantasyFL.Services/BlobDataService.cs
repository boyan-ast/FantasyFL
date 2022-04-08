namespace FantasyFL.Services
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;

    using FantasyFL.Services.Contracts;

    public class BlobDataService : IExternalDataService
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobDataService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<string> GetAllTeamsAsync(int leagueId, int season)
        {
            var fileName = $"teams-{leagueId}-{season}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetSquadAsync(int teamId)
        {
            var fileName = $"players-{teamId}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetRoundsJsonAsync(int league, int season)
        {
            var fileName = $"gameweeks-{season}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetFixturesByRoundAsync(string gameweekName, int season)
        {
            var fileName = $"fixtures-{gameweekName}-{season}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetLineupsJsonAsync(int fixtureId)
        {
            var fileName = $"lineups-{fixtureId}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetFixtureEventsJsonAsync(int fixtureId)
        {
            var fileName = $"events-{fixtureId}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        public async Task<string> GetLeaguesJsonAsync(string countryCode, int season)
        {
            var fileName = $"leagues-{countryCode}-{season}.json";
            var result = await this.GetResult(fileName);

            return result;
        }

        private async Task<string> GetResult(string fileName)
        {
            var container = this.blobServiceClient.GetBlobContainerClient("football-data-jsons");
            var blob = container.GetBlobClient(fileName);

            var blobContent = await blob.DownloadStreamingAsync();

            var result = new StringBuilder();

            using (var streamReader = new StreamReader(blobContent.Value.Content))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    result.AppendLine(line);
                }
            }

            return result.ToString();
        }
    }
}
