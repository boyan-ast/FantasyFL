namespace FantasyFL.Web.ViewModels.Leagues
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class LeagueListingViewModel : IMapFrom<FantasyLeague>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IEnumerable<string> ParticipantsIds { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FantasyLeague, LeagueListingViewModel>()
                .ForMember(x => x.ParticipantsIds, opt =>
                    opt.MapFrom(y => y.ApplicationUsers.Select(u => u.Id)));
        }
    }
}
