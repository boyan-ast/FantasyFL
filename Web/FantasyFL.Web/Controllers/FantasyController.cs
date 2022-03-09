namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;
    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FantasyController : Controller
    {
        private readonly IFantasyTeamService fantasyTeamService;
        private readonly UserManager<ApplicationUser> userManager;

        public FantasyController(
            IFantasyTeamService fantasyTeamService,
            UserManager<ApplicationUser> userManager)
        {
            this.fantasyTeamService = fantasyTeamService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> MyTeam()
        {
            var userId = this.userManager.GetUserId(this.User);

            var players = await this.fantasyTeamService
                .GetUserFantasyPlayers(userId);

            return this.View(players);
        }
    }
}
