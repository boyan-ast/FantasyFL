namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels.FirstLeague;

    using Microsoft.EntityFrameworkCore;

    public class TeamsService : ITeamsService
    {
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public TeamsService(IDeletableEntityRepository<Team> teamsRepository)
        {
            this.teamsRepository = teamsRepository;
        }

        public async Task<List<TeamListingViewModel>> GetAll()
        {
            var teams = await this.teamsRepository
                .All()
                .To<TeamListingViewModel>()
                .ToListAsync();

            return teams;
        }

        public async Task<TeamPlayersViewModel> GetTeamPlayers(int teamId)
        {
            var team = await this.teamsRepository
                .All()
                .Where(t => t.Id == teamId)
                .To<TeamPlayersViewModel>()
                .FirstOrDefaultAsync();

            return team;
        }
    }
}
