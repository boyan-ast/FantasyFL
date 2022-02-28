namespace FantasyFL.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data;

    internal class TeamsSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            if (dbContext.Teams.Any())
            {
                return;
            }

            var seedService = (ISeedService)serviceProvider.GetService(typeof(ISeedService));

            await seedService.ImportTeams();
        }
    }
}
