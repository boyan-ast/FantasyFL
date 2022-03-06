namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class FantasyTeamsService : IFantasyTeamsService
    {
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository;

        public FantasyTeamsService(
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository)
        {
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.fantasyTeamsPlayersRepository = fantasyTeamsPlayersRepository;
        }

        public async Task<FantasyTeam> GetUserTeam(string userId)
        {
            var fantasyTeam = await this.fantasyTeamsRepository
                .All()
                .FirstOrDefaultAsync(t => t.OwnerId == userId);

            return fantasyTeam;
        }

        public async Task<List<Player>> GetFantasyTeamPlayers(string userId)
        {
            var userFantasyTeam = await this.GetUserTeam(userId);

            var players = await this.fantasyTeamsPlayersRepository
                .All()
                .Where(f => f.FantasyTeamId == userFantasyTeam.Id)
                .Select(ftp => ftp.Player)
                .ToListAsync();

            return players;
        }

        public async Task<bool> UserTeamIsEmpty(string userId)
        {
            var isEmpty = !(await this.GetFantasyTeamPlayers(userId)).Any();

            return isEmpty;
        }
    }
}
