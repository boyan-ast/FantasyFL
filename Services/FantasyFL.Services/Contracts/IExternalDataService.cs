namespace FantasyFL.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IExternalDataService
    {
        Task<string> GetSquadAsync(int teamId);

        Task<string> GetAllTeamsAsync(int leagueId, int season);

        Task<string> GetLineupsJsonAsync(int fixtureId);

        Task<string> GetFixtureEventsJsonAsync(int fixtureId);

        Task<string> GetRoundsJsonAsync(int league, int season);

        Task<string> GetLeaguesJsonAsync(string countryCode, int season);

        Task<string> GetFixturesByRoundAsync(string gameweekName, int season);
    }
}
