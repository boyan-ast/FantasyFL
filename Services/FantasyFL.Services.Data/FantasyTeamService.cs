namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fantasy;
    using Microsoft.EntityFrameworkCore;

    public class FantasyTeamService : IFantasyTeamService
    {
        private readonly IGameweekService gameweekService;
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository;
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IRepository<PlayerGameweek> playersGameweeksRepository;

        public FantasyTeamService(
            IGameweekService gameweekService,
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

        public async Task<List<FantasyPlayerViewModel>> GetUserFantasyPlayers(string userId)
        {
            var currentGameweek = this.gameweekService.GetCurrent();

            var userFantasyTeam = await this.GetUserFantasyTeam(userId);

            var userPlayers = await this.fantasyTeamsPlayersRepository
                .All()
                .Where(p => p.FantasyTeamId == userFantasyTeam.Id)
                .ToListAsync();

            var players = new List<FantasyPlayerViewModel>();

            foreach (var userPlayer in userPlayers)
            {
                var playerGameweek = await this.playersGameweeksRepository
                    .All()
                    .Include(p => p.Player)
                    .FirstOrDefaultAsync(p => p.PlayerId == userPlayer.PlayerId
                        && p.GameweekId == currentGameweek.Id);

                if (playerGameweek != null)
                {
                    var player = new FantasyPlayerViewModel
                    {
                        Id = playerGameweek.PlayerId,
                        Name = playerGameweek.Player.Name,
                        GameweekPoints = playerGameweek.TotalPoints,
                        IsPlaying = userPlayer.IsPlaying,
                        FantasyTeam = userFantasyTeam.Name,
                    };

                    players.Add(player);
                }
            }

            return players;
        }

        public async Task<FantasyTeam> GetUserFantasyTeam(string userId)
        {
            var fantasyTeam = await this.fantasyTeamsRepository
                .All()
                .FirstOrDefaultAsync(t => t.OwnerId == userId);

            return fantasyTeam;
        }
    }
}
