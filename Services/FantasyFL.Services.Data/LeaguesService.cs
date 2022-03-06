namespace FantasyFL.Services.Data
{
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
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
    }
}
