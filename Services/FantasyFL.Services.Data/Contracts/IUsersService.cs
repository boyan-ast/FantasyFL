namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<UserTeamViewModel> GetUserTeam(string userId);

        Task AddUserGameweeks(string userId, int startGameweekNumber);

        IEnumerable<UserLeagueListingViewModel> GetUserLeagues(string userId);
    }
}
