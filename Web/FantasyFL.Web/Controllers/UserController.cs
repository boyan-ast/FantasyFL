namespace FantasyFL.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IFantasyTeamsService fantasyTeamsService;

        public UserController(
            IUsersService usersService,
            IFantasyTeamsService fantasyTeamsService)
        {
            this.usersService = usersService;
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

            return this.View(team);
        }
    }
}
