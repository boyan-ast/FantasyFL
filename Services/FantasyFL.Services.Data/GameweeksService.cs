namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Administration.Dashboard;
    using Microsoft.EntityFrameworkCore;

    using static FantasyFL.Common.GlobalConstants;

    public class GameweeksService : IGameweeksService
    {
        private readonly IRepository<Gameweek> gameweekRepository;
        private readonly IGameweekImportService gameweekImportService;
        private readonly IPlayersPointsService playersService;

        public GameweeksService(
            IRepository<Gameweek> gameweekRepository,
            IGameweekImportService gameweekImportService,
            IPlayersPointsService playersService)
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
                .ToListAsync();

            var gameweeksViewModel = new List<GameweekViewModel>();

            foreach (var gameweek in gameweeks)
            {
                gameweeksViewModel.Add(new GameweekViewModel
                {
                    Id = gameweek.Id,
                    Name = gameweek.Name,
                    IsImported = gameweek.IsImported,
                    IsFinished = gameweek.IsFinished,
                    EndDate = gameweek.EndDate,
                    PreviousIsFinished = this.PreviousIsFinished(gameweek.Number),
                });
            }

            return gameweeksViewModel;
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

        public Gameweek GetCurrent()
        {
            var gameweek = this.gameweekRepository
                .AllAsNoTracking()
                .OrderByDescending(gw => gw.Number)
                .FirstOrDefault(gw => gw.IsFinished);

            // TODO: If null season not started
            return gameweek;
        }

        public Gameweek GetNext()
        {
            var gameweek = this.gameweekRepository
                .All()
                .OrderBy(gw => gw.Number)
                .FirstOrDefault(gw => !gw.IsFinished);

            // TODO: If null season over
            return gameweek;
        }

        private bool PreviousIsFinished(int currentNumber)
        {
            var previousGameweek = this.gameweekRepository
                .All()
                .FirstOrDefault(gw => gw.Number == currentNumber - 1);

            return previousGameweek == null || previousGameweek.IsFinished;
        }
    }
}
