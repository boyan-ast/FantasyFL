namespace FantasyFL.Web.ViewModels.FirstLeague
{
    using AutoMapper;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;

    public class PlayerViewModel : IMapFrom<Player>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Team { get; init; }

        public int? Age { get; init; }

        public int? Number { get; init; }

        public Position Position { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Player, PlayerViewModel>()
               .ForMember(x => x.Team, opt =>
                   opt.MapFrom(p => p.Team.Name));
        }
    }
}
