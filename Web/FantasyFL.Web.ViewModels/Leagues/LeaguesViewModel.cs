namespace FantasyFL.Web.ViewModels.Leagues
{
    using System.Collections.Generic;

    public class LeaguesViewModel
    {
        public string UserId { get; init; }

        public IEnumerable<LeagueListingViewModel> Leagues { get; init; }
    }
}
