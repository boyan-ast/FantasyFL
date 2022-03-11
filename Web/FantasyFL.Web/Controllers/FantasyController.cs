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

            var goalkeepers = await this.fantasyTeamService
                .GetUserPlayersByPosition(userId, Position.Goalkeeper);
            var defenders = await this.fantasyTeamService
                .GetUserPlayersByPosition(userId, Position.Defender);
            var midfielders = await this.fantasyTeamService
                .GetUserPlayersByPosition(userId, Position.Midfielder);
            var attackers = await this.fantasyTeamService
                .GetUserPlayersByPosition(userId, Position.Attacker);

            var team = new TeamSelectViewModel
            {
                Goalkeepers = goalkeepers,
                Defenders = defenders,
                Midfielders = midfielders,
                Attackers = attackers,
                SelectedPlayers = new List<int>(),
            };

            if (this.TempData.ContainsKey("errors"))
            {
                this.ViewData["alertMessage"] = this.TempData["errors"].ToString();
                this.TempData.Remove("errors");
            }

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
    }
}
