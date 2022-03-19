namespace FantasyFL.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPlayersService playersService;
        private readonly IFantasyTeamsService fantasyTeamsService;

        public UserController(
            IUsersService usersService,
            IPlayersService playersService,
            IFantasyTeamsService fantasyTeamsService)
        {
            this.usersService = usersService;
            this.playersService = playersService;
            this.fantasyTeamsService = fantasyTeamsService;
        }

        [Authorize]
        public async Task<IActionResult> Team()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await this.fantasyTeamsService.UserTeamIsEmpty(userId))
            {
                return this.Redirect("/PlayersManagement/PickGoalkeepers");
            }

            var team = await this.usersService.GetUserTeam(userId);
            var leagues = this.usersService.GetUserLeagues(userId);

            var model = new UserPageViewModel
            {
                Team = team,
                Leagues = leagues,
            };

            if (this.TempData.ContainsKey("message"))
            {
                this.ViewData["message"] = this.TempData["message"].ToString();
                this.TempData.Clear();
            }

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> PlayerStats(int id)
        {
            var player = await this.playersService.GetPlayerGameweekPerformance(id);

            if (player == null)
            {
                this.TempData["message"] = "Player haven't played this gameweek.";

                return this.RedirectToAction(nameof(this.Team));
            }

            return this.View(player);
        }
    }
}
