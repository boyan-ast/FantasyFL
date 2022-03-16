namespace FantasyFL.Web.ViewModels.Transfers
{
    using System.Collections.Generic;

    using AutoMapper;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;

    public class TeamTransfersViewModel : IMapFrom<ApplicationUserGameweek>, IHaveCustomMappings
    {
        public int GameweekNumber { get; init; }

        public int Transfers { get; init; }

        public IEnumerable<TransferPlayerViewModel> Players { get; init; }

        public int? RemovedPlayerId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserGameweek, TeamTransfersViewModel>()
                .ForMember(x => x.Players, opt =>
                    opt.MapFrom(y => y.User.FantasyTeam.FantasyTeamPlayers));
        }
    }
}
