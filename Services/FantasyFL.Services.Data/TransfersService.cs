namespace FantasyFL.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels.Transfers;
    using Microsoft.EntityFrameworkCore;

    public class TransfersService : ITransfersService
    {
        private readonly IGameweeksService gameweeksService;
        private readonly IUsersService usersService;
        private readonly IRepository<ApplicationUserGameweek> usersGameweeksRepository;
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamPlayerRepository;
        private readonly IDeletableEntityRepository<Player> playersRepository;

        public TransfersService(
            IGameweeksService gameweeksService,
            IUsersService usersService,
            IRepository<ApplicationUserGameweek> usersGameweeksRepository,
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamPlayerRepository,
            IDeletableEntityRepository<Player> playersRepository)
        {
            this.gameweeksService = gameweeksService;
            this.usersService = usersService;
            this.usersGameweeksRepository = usersGameweeksRepository;
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.fantasyTeamPlayerRepository = fantasyTeamPlayerRepository;
            this.playersRepository = playersRepository;
        }

        public async Task AddPlayer(string userId, int playerId)
        {
            var userTeam = await this.usersService.GetUserFantasyTeam(userId);

            userTeam.FantasyTeamPlayers.Add(new FantasyTeamPlayer
            {
                PlayerId = playerId,
            });

            await this.fantasyTeamPlayerRepository.SaveChangesAsync();
        }

        public async Task<List<AddPlayerListingViewModel>> GetPlayersToTransfer(string userId, int removedPlayerId)
        {
            var userTeam = await this.usersService.GetUserFantasyTeam(userId);

            var removedPlayer = await this.playersRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == removedPlayerId);

            var position = removedPlayer.Position;

            var players = await this.GetFilteredPlayers(userTeam, position);

            return players;
        }

        public async Task<TeamTransfersViewModel> GetTransfersList(string userId)
        {
            var gameweekId = this.gameweeksService.GetNext()?.Id;

            if (gameweekId == null)
            {
                throw new InvalidOperationException();
            }

            var userGameweek = await this.usersGameweeksRepository
                .All()
                .Where(g => g.GameweekId == gameweekId && g.UserId == userId)
                .To<TeamTransfersViewModel>()
                .FirstOrDefaultAsync();

            return userGameweek;
        }

        public async Task RemovePlayer(string userId, int playerId)
        {
            var userTeam = await this.usersService.GetUserFantasyTeam(userId);

            var fantasyTeamPlayer = await this.fantasyTeamPlayerRepository
                .All()
                .Where(x => x.FantasyTeamId == userTeam.Id && x.PlayerId == playerId)
                .FirstOrDefaultAsync();

            var gameweekId = this.gameweeksService.GetNext()?.Id;

            if (gameweekId == null)
            {
                throw new InvalidOperationException();
            }

            var userGameweek = await this.usersGameweeksRepository
                .All()
                .Where(u => u.UserId == userId && u.GameweekId == gameweekId)
                .FirstOrDefaultAsync();

            if (userGameweek.Transfers == 0)
            {
                throw new InvalidOperationException();
            }

            userGameweek.Transfers -= 1;

            this.fantasyTeamPlayerRepository.Delete(fantasyTeamPlayer);

            await this.usersGameweeksRepository.SaveChangesAsync();
            await this.fantasyTeamPlayerRepository.SaveChangesAsync();
        }

        private async Task<List<AddPlayerListingViewModel>> GetFilteredPlayers(
            FantasyTeam userTeam,
            Position position)
        {
            var playersTeamsCount = new Dictionary<int, int>();

            foreach (var player in userTeam.FantasyTeamPlayers)
            {
                var playerTeamId = player.Player.TeamId;

                if (!playersTeamsCount.ContainsKey(playerTeamId))
                {
                    playersTeamsCount[playerTeamId] = 0;
                }

                playersTeamsCount[playerTeamId]++;
            }

            var teamsToExcludeIds = new List<int>();

            foreach (var (teamId, count) in playersTeamsCount)
            {
                if (count >= 3)
                {
                    teamsToExcludeIds.Add(teamId);
                }
            }

            var playersToList = (await this.playersRepository
                .AllAsNoTracking()
                .Where(p => p.Position == position && !teamsToExcludeIds.Contains(p.TeamId))
                .To<AddPlayerListingViewModel>()
                .ToListAsync())
                .Where(p => !userTeam.FantasyTeamPlayers.Any(x => x.PlayerId == p.Id))
                .ToList();

            return playersToList;
        }
    }
}
