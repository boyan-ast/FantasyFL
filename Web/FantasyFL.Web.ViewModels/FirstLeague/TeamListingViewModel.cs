namespace FantasyFL.Web.ViewModels.FirstLeague
{
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class TeamListingViewModel : IMapFrom<Team>
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Logo { get; init; }

        public string StadiumName { get; init; }
    }
}
