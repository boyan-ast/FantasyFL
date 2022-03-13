namespace FantasyFL.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fantasy;
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
        public async Task<IActionResult> PickTeam()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userTeamIsEmpty = await this.fantasyTeamService.UserTeamIsEmpty(userId);

            if (userTeamIsEmpty)
            {
                return this.Redirect("/PlayersManagement/PickGoalkeepers");
            }

            if (this.TempData.ContainsKey("errors"))
            {
                this.ViewData["alertMessage"] = this.TempData["errors"].ToString();
                this.TempData.Remove("errors");
            }

            var team = await this.fantasyTeamService.GetUserTeamSelectModel(userId);

            return this.View(team);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickTeam(TeamSelectViewModel team)
        {
            var playingPlayersIds = new HashSet<int>(team.SelectedPlayers);

            if (playingPlayersIds.Count < 11)
            {
                this.TempData["errors"] = "You have to select 11 different players.";
                return this.RedirectToAction(nameof(this.PickTeam));
            }

            var userId = this.userManager.GetUserId(this.User);
            var userTeam = await this.fantasyTeamService.GetUserFantasyTeam(userId);

            await this.fantasyTeamService.ClearUserPlayers(userTeam.Id);
            await this.fantasyTeamService.UpdatePlayingPlayers(userTeam.Id, playingPlayersIds);

            return this.Redirect("/Fixtures/Next");
        }

        [Authorize]
        public async Task<IActionResult> MyTeam()
        {
            var userId = this.userManager.GetUserId(this.User);
            var userGameweekTeam = await this.fantasyTeamService.GetUserGameweekTeam(userId);

            return this.View(userGameweekTeam);
        }
    }
}
