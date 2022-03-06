namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FantasyTeamController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFantasyTeamsService fantasyTeamService;

        public FantasyTeamController(
            UserManager<ApplicationUser> userManager,
            IFantasyTeamsService fantasyTeamService)
        {
            this.userManager = userManager;
            this.fantasyTeamService = fantasyTeamService;
        }

        [Authorize]
        public async Task<IActionResult> PickPlayers()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userTeam = await this.fantasyTeamService.GetUserTeam(userId);

            return this.View(model: userTeam.Name);
        }
    }
}
