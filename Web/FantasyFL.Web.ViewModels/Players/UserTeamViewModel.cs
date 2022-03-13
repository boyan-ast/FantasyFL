namespace FantasyFL.Web.ViewModels.Players
{
    using System.Collections.Generic;

    public class UserTeamViewModel
    {
        public string Name { get; init; }

        public int TotalPoints { get; init; }

        public IEnumerable<PlayerListingViewModel> Players { get; init; }
    }
}
