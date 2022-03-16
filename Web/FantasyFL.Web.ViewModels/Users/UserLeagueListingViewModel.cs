namespace FantasyFL.Web.ViewModels.Users
{
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class UserLeagueListingViewModel : IMapFrom<FantasyLeague>
    {
        public int Id { get; init; }

        public string Name { get; init; }
    }
}
