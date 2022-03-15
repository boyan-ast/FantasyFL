namespace FantasyFL.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly UserManager<ApplicationUser> userManager;

        public FantasyController(
            IFantasyTeamsService fantasyTeamService,
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

            var playingGoalkeepers = team.Goalkeepers.Where(gk => gk.Selected).ToList();
            var playingDefenders = team.Defenders.Where(d => d.Selected).ToList();
            var playingMidfielders = team.Midfielders.Where(m => m.Selected).ToList();
            var playingAttackers = team.Attackers.Where(a => a.Selected).ToList();

            var playingPlayersIds = new HashSet<int>(playingGoalkeepers
                .Concat(playingDefenders)
                .Concat(playingMidfielders)
                .Concat(playingAttackers)
                .Select(p => p.PlayerId));

            var userId = this.userManager.GetUserId(this.User);
            var userTeam = await this.fantasyTeamService.GetUserFantasyTeam(userId);

            await this.fantasyTeamService.ClearUserPlayers(userTeam.Id);
            await this.fantasyTeamService.UpdatePlayingPlayers(userTeam.Id, playingPlayersIds);

            return this.Redirect("/User/Team");
        }

        [Authorize]
        public async Task<IActionResult> GameweekResult()
        {
            var userId = this.userManager.GetUserId(this.User);
            var userGameweekTeam = await this.fantasyTeamService.GetUserGameweekTeam(userId);

            return this.View(userGameweekTeam);
        }
    }
}
