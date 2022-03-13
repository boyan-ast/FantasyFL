namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Web.ViewModels.Fantasy;

    public interface IFantasyTeamService
    {
        Task<TeamPointsViewModel> GetUserGameweekTeam(string userId);

        Task<FantasyTeam> GetUserFantasyTeam(string userId);

        Task<List<PlayerSelectViewModel>> GetUserPlayersByPosition(string userId, Position position);

        Task<bool> UserTeamIsEmpty(string userId);

        Task ClearUserPlayers(string teamId);

        Task UpdatePlayingPlayers(string teamId, HashSet<int> playersIds);

        Task<TeamSelectViewModel> GetUserTeamSelectModel(string userId);
    }
}
