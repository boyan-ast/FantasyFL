namespace FantasyFL.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Services.Mapping;
    using FantasyFL.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository;
        private readonly IRepository<ApplicationUserGameweek> usersGameweeksRepository;
        private readonly IRepository<Gameweek> gameweeksRepository;

        public UsersService(
            IDeletableEntityRepository<FantasyTeam> fantasyTeamsRepository,
            IRepository<ApplicationUserGameweek> usersGameweeksRepository,
            IRepository<Gameweek> gameweeksRepository)
        {
            this.fantasyTeamsRepository = fantasyTeamsRepository;
            this.usersGameweeksRepository = usersGameweeksRepository;
            this.gameweeksRepository = gameweeksRepository;
        }

        public async Task<UserTeamViewModel> GetUserTeam(string userId)
        {
            var team = await this.fantasyTeamsRepository
                .AllAsNoTracking()
                .Where(t => t.OwnerId == userId)
                .To<UserTeamViewModel>()
                .FirstOrDefaultAsync();

            return team;
        }

        public async Task AddUserGameweeks(string userId, int startGameweekNumber)
        {
            var userFutureGameweeksIds = await this.gameweeksRepository
                .AllAsNoTracking()
                .Where(gw => gw.Number >= startGameweekNumber)
                .Select(gw => gw.Id)
                .ToListAsync();

            foreach (var gameweekId in userFutureGameweeksIds)
            {
                await this.usersGameweeksRepository.AddAsync(new ApplicationUserGameweek
                {
                    UserId = userId,
                    GameweekId = gameweekId,
                    Transfers = 1,
                });
            }

            await this.usersGameweeksRepository.SaveChangesAsync();
        }
    }
}
