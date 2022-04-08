namespace FantasyFL.Web.ViewModels.Players
{
    using AutoMapper;

    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;

    public class PlayerListingViewModel : IMapFrom<FantasyTeamPlayer>, IHaveCustomMappings
    {
        public int PlayerId { get; init; }

        public string Name { get; init; }

        public Position Position { get; init; }

        public string Team { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FantasyTeamPlayer, PlayerListingViewModel>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(p => p.Player.Name))
                .ForMember(x => x.Position, opt =>
                    opt.MapFrom(p => p.Player.Position))
                .ForMember(x => x.Team, opt =>
                    opt.MapFrom(p => p.Player.Team.Name));
        }
    }
}
