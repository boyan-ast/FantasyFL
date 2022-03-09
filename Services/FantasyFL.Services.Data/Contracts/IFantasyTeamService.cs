namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Fantasy;

    public interface IFantasyTeamService
    {
        Task<List<FantasyPlayerViewModel>> GetUserFantasyPlayers(string userId);

        Task<FantasyTeam> GetUserFantasyTeam(string userId);
    }
}
