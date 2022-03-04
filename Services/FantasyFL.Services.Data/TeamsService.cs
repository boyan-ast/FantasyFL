namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Teams;
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
                .Select(t => new TeamListingViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Logo = t.Logo,
                    Stadium = t.Stadium.Name,
                })
                .ToListAsync();

            return teams;
        }
    }
}
