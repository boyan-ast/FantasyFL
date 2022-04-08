namespace FantasyFL.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Fantasy;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FantasyController : Controller
    {
        private readonly IFantasyTeamsService fantasyTeamService;
        private readonly IGameweeksService gameweeksService;
        private readonly UserManager<ApplicationUser> userManager;

        public FantasyController(
            IFantasyTeamsService fantasyTeamService,
            IGameweeksService gameweeksService,
            UserManager<ApplicationUser> userManager)
        {
            this.fantasyTeamService = fantasyTeamService;
            this.gameweeksService = gameweeksService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> PickTeam()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userTeamIsEmpty = await this.fantasyTeamService.UserTeamIsEmpty(userId);

            if (userTeamIsEmpty)
            {
                return this.RedirectToAction("PickGoalkeepers", "PlayersManagement");
            }

            var team = await this.fantasyTeamService.GetUserTeamSelectModel(userId);

            return this.View(team);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickTeam(TeamSelectViewModel team)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(team);
            }

            var userId = this.userManager.GetUserId(this.User);
            var userTeam = await this.fantasyTeamService.GetUserFantasyTeam(userId);

            var playingPlayersIds = this.fantasyTeamService.GetPlayingPlayersIds(team);
            await this.fantasyTeamService.ClearUserPlayers(userTeam.Id);
            await this.fantasyTeamService.UpdatePlayingPlayers(userTeam.Id, playingPlayersIds);

            return this.RedirectToAction("Index", "UserTeam");
        }

        [Authorize]
        public async Task<IActionResult> GameweekResult()
        {
            var userId = this.userManager.GetUserId(this.User);
            var userRegisteredBeforeGameweek =
                await this.gameweeksService.UserIsRegisteredBeforeCurrentGameweek(userId);

            if (!userRegisteredBeforeGameweek)
            {
                this.TempData["Message"] = "You haven't participated this gameweek.";
                return this.RedirectToAction("Index", "UserTeam");
            }

            try
            {
                var userGameweekTeam = await this.fantasyTeamService.GetUserGameweekTeam(userId);
                return this.View(userGameweekTeam);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["Message"] = ex.Message;
                return this.RedirectToAction("Index", "UserTeam");
            }
        }
    }
}
