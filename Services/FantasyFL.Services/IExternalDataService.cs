namespace FantasyFL.Services
{
    using System.Threading.Tasks;

    public interface IExternalDataService
    {
        public Task<string> GetSquadAsync(int teamId);

        public Task<string> GetAllTeamsAsync(int leagueId, int season);

        public Task<string> GetLineupsJsonAsync(int fixtureId);

        public Task<string> GetFixtureEventsJsonAsync(int fixtureId);

        public Task<string> GetRoundsJsonAsync(int league, int season);

        public Task<string> GetLeaguesJsonAsync(string countryCode, int season);

        public Task<string> GetFixturesByRoundAsync(string gameweekName, int season);
    }
}
