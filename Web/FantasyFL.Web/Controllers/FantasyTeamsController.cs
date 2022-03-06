namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FantasyTeamsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFantasyTeamsService fantasyTeamService;

        public FantasyTeamsController(
            UserManager<ApplicationUser> userManager,
            IFantasyTeamsService fantasyTeamService)
        {
            this.userManager = userManager;
            this.fantasyTeamService = fantasyTeamService;
        }

        public async Task<IActionResult> PickPlayers()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userTeam = await this.fantasyTeamService.GetUserTeam(userId);

            return this.View(model: userTeam.Name);
        }
    }
}
