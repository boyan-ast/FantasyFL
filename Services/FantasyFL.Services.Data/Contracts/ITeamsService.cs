namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.FirstLeague;

    public interface ITeamsService
    {
        Task<List<TeamListingViewModel>> GetAll();

        Task<TeamPlayersViewModel> GetTeamPlayers(int teamId);
    }
}
