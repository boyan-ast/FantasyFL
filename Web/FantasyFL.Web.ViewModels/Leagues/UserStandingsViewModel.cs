namespace FantasyFL.Web.ViewModels.Leagues
{
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class UserStandingsViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; init; }

        public string FantasyTeamName { get; init; }

        public int TotalPoints { get; init; }
    }
}
