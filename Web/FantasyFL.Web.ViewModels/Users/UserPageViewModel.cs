namespace FantasyFL.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using FantasyFL.Web.ViewModels.Leagues;

    public class UserPageViewModel
    {
        public UserTeamViewModel Team { get; init; }

        public IEnumerable<LeagueListingViewModel> Leagues { get; init; }
    }
}
