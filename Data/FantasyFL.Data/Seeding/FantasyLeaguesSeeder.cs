namespace FantasyFL.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;

    internal class FantasyLeaguesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FantasyLeagues.Any())
            {
                return;
            }

            await dbContext.FantasyLeagues.AddAsync(new FantasyLeague { Name = "Bulgaria" });
        }
    }
}
