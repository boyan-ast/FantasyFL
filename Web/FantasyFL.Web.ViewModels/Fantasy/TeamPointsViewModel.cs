namespace FantasyFL.Web.ViewModels.Fantasy
{
    using System.Collections.Generic;

    public class TeamPointsViewModel
    {
        public string Name { get; init; }

        public int Gameweek { get; init; }

        public IEnumerable<PlayerPointsViewModel> Players { get; init; }
    }
}
