namespace FantasyFL.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static FantasyFL.Common.GlobalConstants;

    internal class AdminUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var userExists = await userManager.Users.AnyAsync(u => u.UserName == AdministratorUserName);

            if (!userExists)
            {
                var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();

                var gameweeksRepository = (IRepository<Gameweek>)serviceProvider
                    .GetService(typeof(IRepository<Gameweek>));
                var usersService = (IUsersService)serviceProvider
                    .GetService(typeof(IUsersService));

                var startGameweek = gameweeksRepository
                        .All()
                        .FirstOrDefault(gw => gw.Number == 1);

                var admin = new ApplicationUser
                {
                    UserName = AdministratorUserName,
                    Email = AdministratorEmail,
                    EmailConfirmed = true,
                    FantasyTeam = new FantasyTeam
                    {
                        Name = "AdminTeam",
                    },
                    StartGameweek = startGameweek,
                };

                var result = await userManager.CreateAsync(admin, AdministratorPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(
                        Environment.NewLine,
                        result.Errors.Select(e => e.Description)));
                }

                var roleExists = await roleManager.RoleExistsAsync(AdministratorRoleName);

                if (roleExists)
                {
                    await userManager.AddToRoleAsync(admin, AdministratorRoleName);
                }

                await usersService.AddUserGameweeks(admin.Id, admin.StartGameweek.Number);
            }
        }
    }
}
