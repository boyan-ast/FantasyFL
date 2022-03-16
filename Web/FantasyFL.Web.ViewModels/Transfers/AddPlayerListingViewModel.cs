namespace FantasyFL.Web.ViewModels.Transfers
{
    using AutoMapper;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Mapping;

    public class AddPlayerListingViewModel : IMapFrom<Player>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public Position Position { get; init; }

        public string Team { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Player, AddPlayerListingViewModel>()
                .ForMember(x => x.Team, opt =>
                    opt.MapFrom(y => y.Team.Name));
        }
    }
}
