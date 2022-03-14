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

        public UserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Authorize]
        public async Task<IActionResult> Team()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var team = await this.usersService.GetUserTeam(userId);

            return this.View(team);
        }
    }
}
