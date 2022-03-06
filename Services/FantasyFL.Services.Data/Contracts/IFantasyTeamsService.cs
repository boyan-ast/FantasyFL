namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;

    public interface IFantasyTeamsService
    {
        Task<FantasyTeam> GetUserTeam(string userId);

        Task<bool> UserTeamIsEmpty(string userId);

        Task<List<Player>> GetFantasyTeamPlayers(string userId);
    }
}
