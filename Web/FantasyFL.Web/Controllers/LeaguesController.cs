namespace FantasyFL.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.Leagues;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class LeaguesController : Controller
    {
        private readonly ILeaguesService leaguesService;

        public LeaguesController(ILeaguesService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var leagues = await this.leaguesService.GetAllLeagues();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new LeaguesViewModel
            {
                UserId = userId,
                Leagues = leagues,
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeagueInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.leaguesService.CreateLeague(input.Name, userId);

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Join(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.leaguesService.JoinLeague(id, userId);

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Standings(int id)
        {
            var standings = await this.leaguesService.GetLeagueStandings(id);

            return this.View(standings);
        }
    }
}
