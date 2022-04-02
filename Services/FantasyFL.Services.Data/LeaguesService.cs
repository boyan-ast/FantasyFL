namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
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
        private readonly IUsersService usersService;

        public LeaguesService(
            IDeletableEntityRepository<FantasyLeague> fantasyLeaguesRepository,
            IUsersService usersService)
        {
            this.fantasyLeaguesRepository = fantasyLeaguesRepository;
            this.usersService = usersService;
        }

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
                .AllAsNoTracking()
                .To<StandingsViewModel>()
                .FirstOrDefaultAsync(l => l.Id == leagueId);

            return standings;
        }

        public async Task<List<LeagueListingViewModel>> GetAllLeagues()
        {
            var leagues = await this.fantasyLeaguesRepository
                .AllAsNoTracking()
                .To<LeagueListingViewModel>()
                .ToListAsync();

            return leagues;
        }

        public async Task CreateLeague(CreateLeagueInputModel leagueModel, string userId)
        {
            var league = new FantasyLeague
            {
                Name = leagueModel.Name,
                Description = leagueModel.Description,
            };

            await this.fantasyLeaguesRepository.AddAsync(league);

            await this.fantasyLeaguesRepository.SaveChangesAsync();

            await this.JoinLeague(league.Id, userId);
        }

        public async Task JoinLeague(int leagueId, string userId)
        {
            var league = await this.fantasyLeaguesRepository
                .All()
                .Include(l => l.ApplicationUsers)
                .FirstOrDefaultAsync(l => l.Id == leagueId);

            if (league.ApplicationUsers.Any(u => u.Id == userId))
            {
                throw new InvalidOperationException("User already joined this league");
            }

            var user = await this.usersService.GetUserById(userId);

            league.ApplicationUsers.Add(user);

            await this.fantasyLeaguesRepository.SaveChangesAsync();
        }
    }
}
