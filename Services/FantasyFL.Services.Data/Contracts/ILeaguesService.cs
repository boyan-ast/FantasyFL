namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Leagues;

    public interface ILeaguesService
    {
        Task<FantasyLeague> GetLeagueByName(string leagueName);

        Task<StandingsViewModel> GetLeagueStandings(int leagueId);

        Task<List<LeagueListingViewModel>> GetAllLeagues();

        Task CreateLeague(CreateLeagueInputModel model, string userId);

        Task JoinLeague(int leagueId, string userId);
    }
}
