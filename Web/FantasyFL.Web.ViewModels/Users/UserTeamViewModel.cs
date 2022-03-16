﻿namespace FantasyFL.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using AutoMapper;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels.Players;

    public class UserTeamViewModel : IMapFrom<FantasyTeam>, IHaveCustomMappings
    {
        public string Name { get; init; }

        public int TotalPoints { get; init; }

        public IEnumerable<PlayerListingViewModel> FantasyTeamPlayers { get; init; }

        public IEnumerable<UserLeagueListingViewModel> FantasyLeagues { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FantasyTeam, UserTeamViewModel>()
                .ForMember(x => x.TotalPoints, opt =>
                    opt.MapFrom(x => x.Owner.TotalPoints));
        }
    }
}
