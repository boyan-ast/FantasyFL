namespace FantasyFL.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class FirstLeagueController : Controller
    {
        private readonly IFixturesService fixturesService;

        public FirstLeagueController(IFixturesService fixturesService)
        {
            this.fixturesService = fixturesService;
        }

        public async Task<IActionResult> Results()
        {
            var fixtures = await this.fixturesService.GetAllInCurrentGameweek();

            if (!fixtures.Any())
            {
                return this.Redirect("/FirstLeague/Fitures");
            }

            return this.View(fixtures);
        }

        public async Task<IActionResult> Fixtures()
        {
            var fixtures = await this.fixturesService.GetAllInNextGameweek();

            return this.View(fixtures);
        }
    }
}
