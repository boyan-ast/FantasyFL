namespace FantasyFL.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class UserPageViewModel
    {
        public UserTeamViewModel Team { get; init; }

        public IEnumerable<UserLeagueListingViewModel> Leagues { get; init; }
    }
}
