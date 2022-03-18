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
    using FantasyFL.Services;
    using FantasyFL.Services.Contracts;
    using FantasyFL.Services.Data;
    using FantasyFL.Services.Data.Contracts;
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

        private static async Task<int> SandboxCode(
            SandboxOptions options,
            IServiceProvider serviceProvider)
        {
            var input = "https://media.api-sports.io/football/teams/853.png";
            var result = input.Split("teams/")[1];
            Console.WriteLine(result);
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
            services.AddTransient<IParseService, ParseService>();
            services.AddTransient<IExternalDataService, JsonDataService>();
            services.AddTransient<IFootballDataService, FootballDataService>();
            services.AddTransient<IGameweekImportService, GameweekImportService>();
            services.AddTransient<IPlayersPointsService, PlayersPointsService>();
            services.AddTransient<IGameweeksService, GameweeksService>();
            services.AddTransient<IFixturesService, FixturesService>();
            services.AddTransient<IPlayersService, PlayersService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<ILeaguesService, LeaguesService>();
            services.AddTransient<ISeedService, SeedService>();
            services.AddTransient<IPlayersManagementService, PlayersManagementService>();
            services.AddTransient<IFantasyTeamsService, FantasyTeamsService>();
            services.AddTransient<IUsersService, UsersService>();
        }
    }
}
