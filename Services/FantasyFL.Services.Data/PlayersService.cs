namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Players;
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

        public async Task<List<PlayerListingViewModel>> GetAllByTeam(int id)
        {
            var players = await this.playersRepository
                .All()
                .Where(p => p.TeamId == id)
                .OrderBy(p => p.Position)
                .Select(p => new PlayerListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position.ToString(),
                    Team = p.Team.Name,
                })
                .ToListAsync();

            return players;
        }
    }
}
