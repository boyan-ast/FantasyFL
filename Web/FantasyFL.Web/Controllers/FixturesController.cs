namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class FixturesController : Controller
    {
        private readonly IFixturesService fixturesService;

        public FixturesController(IFixturesService fixturesService)
        {
            this.fixturesService = fixturesService;
        }

        public async Task<IActionResult> Current()
        {
            var fixtures = await this.fixturesService.GetAllInCurrentGameweek();

            return this.View(fixtures);
        }

        public async Task<IActionResult> Next()
        {
            var fixtures = await this.fixturesService.GetAllInNextGameweek();

            return this.View(fixtures);
        }
    }
}
