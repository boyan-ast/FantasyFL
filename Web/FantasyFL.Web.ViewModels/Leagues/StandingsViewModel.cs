namespace FantasyFL.Web.ViewModels.Leagues
{
    using System.Collections.Generic;

    using AutoMapper;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class StandingsViewModel : IMapFrom<FantasyLeague>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        public int Participants { get; init; }

        public IEnumerable<UserStandingsViewModel> ApplicationUsers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FantasyLeague, StandingsViewModel>()
                .ForMember(x => x.Participants, opt =>
                    opt.MapFrom(x => x.ApplicationUsers.Count));
        }
    }
}
