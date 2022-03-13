namespace FantasyFL.Web.ViewModels.Fantasy
{
    using System.Collections.Generic;

    public class UserTeamViewModel
    {
        public string Name { get; init; }

        public IEnumerable<UserPlayerViewModel> Players { get; init; }
    }
}
