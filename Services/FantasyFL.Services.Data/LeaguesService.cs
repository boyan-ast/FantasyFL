namespace FantasyFL.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels.Leagues;
    using Microsoft.EntityFrameworkCore;

    public class LeaguesService : ILeaguesService
    {
        private readonly IDeletableEntityRepository<FantasyLeague> fantasyLeaguesRepository;

        public LeaguesService(IDeletableEntityRepository<FantasyLeague> fantasyLeaguesRepository)
        {
            this.fantasyLeaguesRepository = fantasyLeaguesRepository;
        }

        // TODO: Make all firstordefault - async
        public async Task<FantasyLeague> GetLeagueByName(string leagueName)
        {
            var league = await this.fantasyLeaguesRepository
                .All()
                .FirstOrDefaultAsync(fl => fl.Name == leagueName);

            return league;
        }

        public async Task<StandingsViewModel> GetLeagueStandings(int leagueId)
        {
            var standings = await this.fantasyLeaguesRepository
                .All()
                .To<StandingsViewModel>()
                .FirstOrDefaultAsync(l => l.Id == leagueId);

            return standings;
        }
    }
}
