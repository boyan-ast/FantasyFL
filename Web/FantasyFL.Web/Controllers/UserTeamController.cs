namespace FantasyFL.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UserTeamController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPlayersService playersService;
        private readonly IFantasyTeamsService fantasyTeamsService;

        public UserTeamController(
            IUsersService usersService,
            IPlayersService playersService,
            IFantasyTeamsService fantasyTeamsService)
        {
            this.usersService = usersService;
            this.playersService = playersService;
            this.fantasyTeamsService = fantasyTeamsService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await this.fantasyTeamsService.UserTeamIsEmpty(userId))
            {
                return this.Redirect("/PlayersManagement/PickGoalkeepers");
            }

            var team = await this.usersService.GetUserTeamViewModel(userId);
            var leagues = this.usersService.GetUserLeagues(userId);

            var model = new UserPageViewModel
            {
                Team = team,
                Leagues = leagues,
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> PlayerStats(int id)
        {
            var player = await this.playersService.GetPlayerGameweekPerformance(id);

            if (player == null)
            {
                this.TempData["Message"] = "Player haven't played this gameweek.";

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(player);
        }
    }
}
