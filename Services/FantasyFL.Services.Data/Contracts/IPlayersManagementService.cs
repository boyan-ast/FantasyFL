namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.PlayersManagement;

    public interface IPlayersManagementService
    {
        Task AddPlayersToTeam(PickPlayersFormModel model, string ownerId);

        Task<FantasyTeam> GetUserTeam(string userId);

        Task<bool> UserTeamIsEmpty(string userId);

        Task<List<Player>> GetFantasyTeamPlayers(string userId);

        // TODO: Remove public from interfaces
    }
}
