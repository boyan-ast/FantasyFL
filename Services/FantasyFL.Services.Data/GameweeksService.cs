namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Administration.Data;
    using Microsoft.EntityFrameworkCore;

    using static FantasyFL.Common.GlobalConstants;

    public class GameweeksService : IGameweeksService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Gameweek> gameweekRepository;
        private readonly IRepository<PlayerGameweek> playersGameweeksRepository;
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository;
        private readonly IRepository<ApplicationUserGameweek> usersGameweeksRepository;
        private readonly IGameweekImportService gameweekImportService;
        private readonly IPlayersPointsService playersPointsService;

        public GameweeksService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<Gameweek> gameweekRepository,
            IRepository<PlayerGameweek> playersGameweeksRepository,
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository,
            IRepository<ApplicationUserGameweek> usersGameweeksRepository,
            IGameweekImportService gameweekImportService,
            IPlayersPointsService playersService)
        {
            this.usersRepository = usersRepository;
            this.gameweekRepository = gameweekRepository;
            this.playersGameweeksRepository = playersGameweeksRepository;
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.fantasyTeamsPlayersRepository = fantasyTeamsPlayersRepository;
            this.usersGameweeksRepository = usersGameweeksRepository;
            this.gameweekImportService = gameweekImportService;
            this.playersPointsService = playersService;
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
            await this.playersPointsService.CalculatePoints(gameweekId);

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

            var usersPlayingInGameweek = await this.usersRepository
                .All()
                .Where(u => u.StartGameweek.Number <= gameweek.Number)
                .ToListAsync();

            foreach (var user in usersPlayingInGameweek)
            {
                var userGameweek = await this.usersGameweeksRepository
                    .All()
                    .FirstOrDefaultAsync(u => u.UserId == user.Id && u.GameweekId == gameweekId);

                if (userGameweek != null)
                {
                    var points = await this.CalculateUserGameweekPoints(user.Id, gameweekId);
                    userGameweek.Points = points;
                    user.TotalPoints += points;
                }
            }

            await this.usersGameweeksRepository.SaveChangesAsync();
            await this.usersRepository.SaveChangesAsync();
            await this.gameweekRepository.SaveChangesAsync();
        }

        public async Task<int> CalculateUserGameweekPoints(string userId, int gameweekId)
        {
            var userFantasyTeam = await this.fantasyTeamsRepository
                .All()
                .FirstOrDefaultAsync(t => t.OwnerId == userId);

            var userPlayingPlayersIds = await this.fantasyTeamsPlayersRepository
                .AllAsNoTracking()
                .Where(p => p.FantasyTeamId == userFantasyTeam.Id && p.IsPlaying)
                .Select(p => p.PlayerId)
                .ToListAsync();

            var points = await this.playersGameweeksRepository
                .AllAsNoTracking()
                .Where(p => p.GameweekId == gameweekId && userPlayingPlayersIds.Contains(p.PlayerId))
                .SumAsync(p => p.TotalPoints);

            return points;
        }

        public Gameweek GetCurrent()
        {
            var gameweek = this.gameweekRepository
                .All()
                .OrderByDescending(gw => gw.Number)
                .FirstOrDefault(gw => gw.IsFinished);

            if (gameweek == null)
            {
                throw new InvalidOperationException("Season not started yet.");
            }

            return gameweek;
        }

        public Gameweek GetNext()
        {
            var gameweek = this.gameweekRepository
                .All()
                .OrderBy(gw => gw.Number)
                .FirstOrDefault(gw => !gw.IsFinished);

            if (gameweek == null)
            {
                throw new InvalidOperationException("The season is over.");
            }

            return gameweek;
        }

        public async Task<bool> UserIsRegisteredBeforeCurrentGameweek(string userId)
        {
            var currentGameweekId = this.GetCurrent().Number;
            var userIsRegisteredBeforeGameweek = await this.usersGameweeksRepository
                .AllAsNoTracking()
                .AnyAsync(gw => gw.UserId == userId && gw.GameweekId == currentGameweekId);

            return userIsRegisteredBeforeGameweek;
        }

        public async Task<int> GetGameweekNumberById(int id)
        {
            var gameweek = await this.gameweekRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(gw => gw.Id == id);

            return gameweek.Number;
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
