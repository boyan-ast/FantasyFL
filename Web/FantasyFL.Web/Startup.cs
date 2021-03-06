namespace FantasyFL.Web
{
    using System.Reflection;

    using Azure.Storage.Blobs;

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
    using FantasyFL.Services.Mapping;
    using FantasyFL.Services.Messaging;
    using FantasyFL.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using static FantasyFL.Common.GlobalConstants;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(
                options =>
                {
                    options.Password.RequiredLength = PasswordMinLength;
                });

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IParseService, ParseService>();
            services.AddTransient<IExternalDataService, BlobDataService>();
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
            services.AddTransient<ITransfersService, TransfersService>();

            services.AddSingleton(x =>
                new BlobServiceClient(this.configuration.GetValue<string>("FantasyBlobConnectionString")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/StatusCodeError");
                app.UseStatusCodePagesWithReExecute("/Home/StatusCodeError", "errorCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints
                            .MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints
                            .MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints
                            .MapRazorPages();
                    });
        }
    }
}
