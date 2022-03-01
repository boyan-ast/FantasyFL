namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CommandLine;
    using FantasyFL.Data;
    using FantasyFL.Data.Common;
    using FantasyFL.Data.Common.Repositories;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Repositories;
    using FantasyFL.Data.Seeding;
    using FantasyFL.Services.Data;
    using FantasyFL.Services.Messaging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                    _ => 255);
            }
        }

        private static async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();

            var settingsService = serviceProvider.GetService<ISettingsService>();
            Console.WriteLine($"Count of settings: {settingsService.GetCount()}");

            var data = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            var playersGameweekOne = data
                .PlayersGameweeks
                .Where(pg => pg.GameweekId == 19 && pg.Player.TeamId == 3)
                .Select(pg => new
                {
                    Player = pg.Player.Name,
                    PlayerTeam = pg.Player.Team.Name,
                    pg.InStartingLineup,
                    pg.IsSubstitute,
                    pg.MinutesPlayed,
                    pg.Goals,
                    pg.YellowCards,
                    pg.RedCards,
                    pg.CleanSheet,
                    pg.ConcededGoals,
                    pg.TotalPoints,
                    PlayerTeamWon = pg.TeamResult,
                })
                .OrderByDescending(p => p.TotalPoints)
                .ToList();

            foreach (var player in playersGameweekOne)
            {
                Console.WriteLine($"Name : {player.Player}");
                Console.WriteLine($"Minutes played: {player.MinutesPlayed}");
                Console.WriteLine($"Goals: {player.Goals}");
                Console.WriteLine($"Yellow cards: {player.YellowCards}");
                Console.WriteLine($"Red cards: {player.RedCards}");
                Console.WriteLine($"Has clean sheet {player.CleanSheet}");
                Console.WriteLine($"Conceded goals: {player.ConcededGoals}");
                Console.WriteLine($"Total points for gameweek: {player.TotalPoints}");
                Console.WriteLine(new string('*', 10));
                Console.WriteLine($"Player team: {player.PlayerTeam} -> {player.PlayerTeamWon}");
                Console.WriteLine(new string('*', 10));
                Console.WriteLine(new string('*', 10));
            }

            Console.WriteLine(sw.Elapsed);
            return await Task.FromResult(0);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(new LoggerFactory()));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
        }
    }
}
