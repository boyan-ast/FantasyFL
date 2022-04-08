namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.PlayersManagement;

    using Microsoft.EntityFrameworkCore;

    public class PlayersManagementService : IPlayersManagementService
    {
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository;

        public PlayersManagementService(
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IDeletableEntityRepository<FantasyTeamPlayer> fantasyTeamsPlayersRepository)
        {
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.fantasyTeamsPlayersRepository = fantasyTeamsPlayersRepository;
        }

        public async Task AddPlayersToTeam(PickPlayersFormModel model, string ownerId)
        {
            var fantasyTeam = await this.fantasyTeamsRepository
                .All()
                .Where(t => t.OwnerId == ownerId)
                .FirstOrDefaultAsync();

            this.AddPlayersLinesToTeam(model.Goalkeepers, fantasyTeam);

            this.AddPlayersLinesToTeam(model.Defenders, fantasyTeam);

            this.AddPlayersLinesToTeam(model.Midfielders, fantasyTeam);

            this.AddPlayersLinesToTeam(model.Attackers, fantasyTeam);

            await this.fantasyTeamsRepository.SaveChangesAsync();
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

        private void AddPlayersLinesToTeam(List<PlayerInputModel> players, FantasyTeam fantasyTeam)
        {
            foreach (var player in players)
            {
                fantasyTeam
                    .FantasyTeamPlayers
                    .Add(new FantasyTeamPlayer
                    {
                        PlayerId = player.Id,
                        IsPlaying = false,
                    });
            }
        }
    }
}
