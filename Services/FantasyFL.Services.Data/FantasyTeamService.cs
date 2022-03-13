namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fantasy;
    using Microsoft.EntityFrameworkCore;

    public class FantasyTeamService : IFantasyTeamService
    {
        private readonly IGameweeksService gameweekService;
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository;
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IRepository<PlayerGameweek> playersGameweeksRepository;

        public FantasyTeamService(
            IGameweeksService gameweekService,
            IDeletableEntityRepository<Player> playersRepository,
            IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository,
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IRepository<PlayerGameweek> playersGameweeksRepository)
        {
            this.gameweekService = gameweekService;
            this.playersRepository = playersRepository;
            this.fantasyTeamsPlayersRepository = fantasyTeamsPlayersRepository;
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.playersGameweeksRepository = playersGameweeksRepository;
        }

        public async Task<UserTeamViewModel> GetUserGameweekTeam(string userId)
        {
            var currentGameweek = this.gameweekService.GetCurrent();

            var userFantasyTeam = await this.GetUserFantasyTeam(userId);

            var userPlayers = await this.fantasyTeamsPlayersRepository
                .AllAsNoTracking()
                .Include(p => p.Player)
                .Where(p => p.FantasyTeamId == userFantasyTeam.Id)
                .ToListAsync();

            if (currentGameweek == null)
            {
                var userTeam = new UserTeamViewModel
                {
                    Name = userFantasyTeam.Name,
                    Gameweek = 0,
                    Players = userPlayers
                        .Select(p => new UserPlayerViewModel
                        {
                            Name = p.Player.Name,
                            Position = p.Player.Position.ToString(),
                            GameweekPoints = 0,
                            IsPlaying = false,
                        })
                        .ToList(),
                };

                return userTeam;
            }
            else
            {
                var players = new List<UserPlayerViewModel>();

                foreach (var userPlayer in userPlayers)
                {
                    var playerGameweek = await this.playersGameweeksRepository
                        .AllAsNoTracking()
                        .FirstOrDefaultAsync(p => p.PlayerId == userPlayer.PlayerId
                            && p.GameweekId == currentGameweek.Id);

                    var player = new UserPlayerViewModel
                    {
                        Name = userPlayer.Player.Name,
                        Position = userPlayer.Player.Position.ToString(),
                        GameweekPoints = playerGameweek != null ? playerGameweek.TotalPoints : 0,
                        IsPlaying = userPlayer.IsPlaying,
                    };

                    players.Add(player);
                }

                var userTeam = new UserTeamViewModel
                {
                    Name = userFantasyTeam.Name,
                    Gameweek = currentGameweek.Number,
                    Players = players,
                };

                return userTeam;
            }
        }

        public async Task<FantasyTeam> GetUserFantasyTeam(string userId)
        {
            var fantasyTeam = await this.fantasyTeamsRepository
                .All()
                .FirstOrDefaultAsync(t => t.OwnerId == userId);

            return fantasyTeam;
        }

        public async Task<bool> UserTeamIsEmpty(string userId)
        {
            var team = await this.fantasyTeamsRepository
                .All()
                .Where(t => t.OwnerId == userId)
                .FirstOrDefaultAsync(t => t.FantasyTeamPlayers.Any());

            return team == null;
        }

        public async Task<TeamSelectViewModel> GetUserTeamSelectModel(string userId)
        {
            var goalkeepers = await this.GetUserPlayersByPosition(userId, Position.Goalkeeper);
            var defenders = await this.GetUserPlayersByPosition(userId, Position.Defender);
            var midfielders = await this.GetUserPlayersByPosition(userId, Position.Midfielder);
            var attackers = await this.GetUserPlayersByPosition(userId, Position.Attacker);

            var team = new TeamSelectViewModel
            {
                Goalkeepers = goalkeepers,
                Defenders = defenders,
                Midfielders = midfielders,
                Attackers = attackers,
            };

            return team;
        }

        public async Task<List<PlayerSelectViewModel>> GetUserPlayersByPosition(string userId, Position position)
        {
            var userFantasyTeam = await this.GetUserFantasyTeam(userId);

            var players = await this.fantasyTeamsPlayersRepository
                .All()
                .Where(p => p.FantasyTeamId == userFantasyTeam.Id && p.Player.Position == position)
                .Select(p => new PlayerSelectViewModel
                {
                    PlayerId = p.PlayerId,
                    Name = p.Player.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task ClearUserPlayers(string teamId)
        {
            var players = await this.fantasyTeamsPlayersRepository
                .All()
                .Where(p => p.FantasyTeamId == teamId)
                .ToListAsync();

            foreach (var player in players)
            {
                player.IsPlaying = false;
            }

            await this.fantasyTeamsRepository.SaveChangesAsync();
        }

        public async Task UpdatePlayingPlayers(string teamId, HashSet<int> playersIds)
        {
            var players = await this.fantasyTeamsPlayersRepository
                .All()
                .Where(p => p.FantasyTeamId == teamId && playersIds.Contains(p.PlayerId))
                .ToListAsync();

            foreach (var player in players)
            {
                player.IsPlaying = true;
            }

            await this.fantasyTeamsRepository.SaveChangesAsync();
        }
    }
}
