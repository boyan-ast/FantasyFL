namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.PlayersManagement;
    using FantasyFL.Web.ViewModels.Teams;
    using Microsoft.EntityFrameworkCore;

    public class PlayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> playersRepository;
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public PlayersService(
            IDeletableEntityRepository<Player> playersRepository,
            IDeletableEntityRepository<Team> teamsRepository)
        {
            this.playersRepository = playersRepository;
            this.teamsRepository = teamsRepository;
        }

        public async Task<List<TeamPlayerViewModel>> GetAllByTeam(int id)
        {
            var players = await this.playersRepository
                .All()
                .Where(p => p.TeamId == id)
                .OrderBy(p => p.Position)
                .Select(p => new TeamPlayerViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<List<TeamPlayerViewModel>> GetAllPlayers()
        {
            var players = await this.playersRepository
                .All()
                .OrderBy(p => p.TeamId)
                .ThenBy(p => p.Position)
                .Select(p => new TeamPlayerViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<List<TeamPlayerViewModel>> GetAllByPosition(Position position)
        {
            var players = await this.playersRepository
                .All()
                .Where(p => p.Position == position)
                .OrderBy(p => p.TeamId)
                .Select(p => new TeamPlayerViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }

        public async Task<string> GetPlayerTeamName(int playerId)
        {
            var teamName = await this.playersRepository
                .All()
                .Where(p => p.Id == playerId)
                .Select(p => p.Team.Name)
                .FirstOrDefaultAsync();

            return teamName;
        }

        public async Task<int> GetPlayerIdByName(string playerName)
        {
            var player = await this.playersRepository
                .All()
                .FirstOrDefaultAsync(p => p.Name == playerName);

            return player.Id;
        }

        public async Task<IDictionary<string, int>> GetPlayersTeamsCount(PickPlayersFormModel model)
        {
            var teamsPlayers = new Dictionary<string, int>();

            foreach (var player in model.Goalkeepers)
            {
                await this.AddPlayerToTeamsPlayers(player, teamsPlayers);
            }

            foreach (var player in model.Defenders)
            {
                await this.AddPlayerToTeamsPlayers(player, teamsPlayers);
            }

            foreach (var player in model.Midfielders)
            {
                await this.AddPlayerToTeamsPlayers(player, teamsPlayers);
            }

            foreach (var player in model.Attackers)
            {
                await this.AddPlayerToTeamsPlayers(player, teamsPlayers);
            }

            return teamsPlayers;
        }

        private async Task AddPlayerToTeamsPlayers(PlayerInputModel player, Dictionary<string, int> teamsPlayers)
        {
            var playerTeam = await this.GetPlayerTeamName(player.Id);

            if (!teamsPlayers.ContainsKey(playerTeam))
            {
                teamsPlayers[playerTeam] = 0;
            }

            teamsPlayers[playerTeam]++;
        }
    }
}
