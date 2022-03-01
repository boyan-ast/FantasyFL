namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Admin;
    using Microsoft.EntityFrameworkCore;

    using static FantasyFL.Common.GlobalConstants;

    public class GameweekService : IGameweekService
    {
        private readonly IRepository<Gameweek> gameweekRepository;
        private readonly IGameweekImportService gameweekImportService;
        private readonly IPlayersService playersService;

        public GameweekService(
            IRepository<Gameweek> gameweekRepository,
            IGameweekImportService gameweekImportService,
            IPlayersService playersService)
        {
            this.gameweekRepository = gameweekRepository;
            this.gameweekImportService = gameweekImportService;
            this.playersService = playersService;
        }

        public async Task<List<GameweekViewModel>> GetAllAsync()
        {
            var gameweeks = await this.gameweekRepository
                .All()
                .OrderBy(gw => gw.Number)
                .Select(gw => new GameweekViewModel
                {
                    Id = gw.Id,
                    Name = gw.Name,
                    IsImported = gw.IsImported,
                    IsFinished = gw.IsFinished,
                })
                .ToListAsync();

            return gameweeks;
        }

        public async Task GetPlayersData(int gameweekId)
        {
            var gameweek = await this.gameweekRepository
                .All()
                .FirstOrDefaultAsync(gw => gw.Id == gameweekId);

            if (gameweek.EndDate > DateTime.UtcNow)
            {
                throw new InvalidOperationException($"The matches in gameweek '{gameweek.Name}' haven't been played yet.");
            }

            await this.gameweekImportService.ImportFixtures(gameweek.Name, SeasonYear);
            await this.gameweekImportService.ImportLineups(gameweekId);
            await this.gameweekImportService.ImportEvents(gameweekId);
            await this.playersService.CalculatePoints(gameweekId);

            gameweek.IsImported = true;

            await this.gameweekRepository.SaveChangesAsync();
        }

        public async Task FinishGameweek(int gameweekId)
        {
            var gameweek = await this.gameweekRepository
                .All()
                .FirstOrDefaultAsync(gw => gw.Id == gameweekId);

            if (!gameweek.IsImported)
            {
                throw new InvalidOperationException($"Gameweek '{gameweek.Name}' must be imported first.");
            }

            gameweek.IsFinished = true;

            await this.gameweekRepository.SaveChangesAsync();
        }
    }
}
