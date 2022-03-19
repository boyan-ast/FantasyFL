namespace FantasyFL.Web.ViewModels.Players
{
    using AutoMapper;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;

    public class PlayerGameweekViewModel : IMapFrom<PlayerGameweek>, IHaveCustomMappings
    {
        public string Name { get; init; }

        public string Team { get; init; }

        public int MinutesPlayed { get; init; }

        public int Goals { get; init; }

        public int YellowCards { get; init; }

        public int RedCards { get; init; }

        public int ConcededGoals { get; set; }

        public int TotalPoints { get; set; }

        public TeamResult? TeamResult { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PlayerGameweek, PlayerGameweekViewModel>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(p => p.Player.Name))
                .ForMember(x => x.Team, opt =>
                    opt.MapFrom(p => p.Player.Team.Name));
        }
    }
}
