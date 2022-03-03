namespace FantasyFL.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;

    internal class PlayersSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            if (dbContext.Players.Any())
            {
                return;
            }

            var seedService = (ISeedService)serviceProvider.GetService(typeof(ISeedService));
            var footballDataService = (IFootballDataService)serviceProvider.GetService(typeof(IFootballDataService));

            await seedService.ImportPlayers();
            await footballDataService.SetTeamsTopPlayers();
        }
    }
}
