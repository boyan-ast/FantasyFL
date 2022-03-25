namespace FantasyFL.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Leagues;
    using FantasyFL.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<FantasyTeam> GetUserFantasyTeam(string userId);

        Task<UserTeamViewModel> GetUserTeamViewModel(string userId);

        Task AddUserGameweeks(string userId, int startGameweekNumber);

        IEnumerable<LeagueListingViewModel> GetUserLeagues(string userId);

        Task<ApplicationUser> GetUserById(string userId);
    }
}
