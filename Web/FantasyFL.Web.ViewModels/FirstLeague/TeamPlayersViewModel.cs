namespace FantasyFL.Web.ViewModels.FirstLeague
{
    using System.Collections.Generic;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class TeamPlayersViewModel : IMapFrom<Team>
    {
        public string Name { get; init; }

        public string Logo { get; init; }

        public string StadiumName { get; init; }

        public IEnumerable<PlayerViewModel> Players { get; init; }
    }
}
