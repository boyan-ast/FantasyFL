namespace FantasyFL.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFantasyTeamsService fantasyTeamsService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IFantasyTeamsService fantasyTeamsService)
        {
            this.userManager = userManager;
            this.fantasyTeamsService = fantasyTeamsService;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(this.User);
                var isUserTeamEmpty = await this.fantasyTeamsService.UserTeamIsEmpty(userId);

                if (isUserTeamEmpty)
                {
                    return this.Redirect("/FantasyTeam/PickGoalkeepers");
                }
                else
                {
                    return this.Redirect("/");
                }
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
