namespace FantasyFL.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;

    internal class FixturesSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            if (dbContext.Fixtures.Any())
            {
                return;
            }

            var seedService = (ISeedService)serviceProvider.GetService(typeof(ISeedService));

            await seedService.ImportFixtures();
        }
    }
}
