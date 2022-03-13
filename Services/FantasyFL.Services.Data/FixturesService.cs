namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fixtures;
    using Microsoft.EntityFrameworkCore;

    public class FixturesService : IFixturesService
    {
        private readonly IGameweeksService gameweekService;
        private readonly IRepository<Fixture> fixturesRepository;

        public FixturesService(
            IGameweeksService gameweekService,
            IRepository<Fixture> fixturesRepository)
        {
            this.gameweekService = gameweekService;
            this.fixturesRepository = fixturesRepository;
        }

        public async Task<List<FixtureViewModel>> GetAllInCurrentGameweek()
        {
            var currentGameweek = this.gameweekService.GetCurrent();

            if (currentGameweek == null)
            {
                return new List<FixtureViewModel>();
            }

            var fixtures = await this.GetFixturesInGameweek(currentGameweek.Id);

            return fixtures;
        }

        public async Task<List<FixtureViewModel>> GetAllInNextGameweek()
        {
            var nextGameweek = this.gameweekService.GetNext();

            var fixtures = await this.GetFixturesInGameweek(nextGameweek.Id);

            return fixtures;
        }

        public async Task<List<FixtureViewModel>> GetFixturesInGameweek(int gameweekId)
        {
            var fixtures = await this.fixturesRepository
                .All()
                .Where(f => f.GameweekId == gameweekId)
                .Select(f => new FixtureViewModel
                {
                    Date = f.Date == null ? null : ((DateTime)f.Date).ToString("dd-MM-yyyy"),
                    HomeTeam = f.HomeTeam.Name,
                    HomeTeamLogo = f.HomeTeam.Logo,
                    AwayTeam = f.AwayTeam.Name,
                    AwayTeamLogo = f.AwayTeam.Logo,
                    HomeGoals = f.HomeGoals,
                    AwayGoals = f.AwayGoals,
                    Stadium = f.HomeTeam.Stadium.Name,
                })
                .ToListAsync();

            return fixtures;
        }
    }
}
