namespace FantasyFL.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Web.ViewModels.Admin;
    using Microsoft.EntityFrameworkCore;

    public class GameweekService : IGameweekService
    {
        private readonly IRepository<Gameweek> gameweekRepository;

        public GameweekService(IRepository<Gameweek> gameweekRepository)
        {
            this.gameweekRepository = gameweekRepository;
        }

        public async Task<List<GameweekViewModel>> GetAllAsync()
        {
            var gameweeks = await this.gameweekRepository
                .All()
                .OrderBy(gw => gw.Number)
                .Select(gw => new GameweekViewModel
                {
                    Name = gw.Name,
                })
                .ToListAsync();

            return gameweeks;
        }
    }
}
