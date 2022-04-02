namespace FantasyFL.Web.Controllers
{
    using System;
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
            try
            {
                var fixtures = await this.fixturesService.GetAllInCurrentGameweek();
                return this.View(fixtures);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["Message"] = ex.Message;
                return this.RedirectToAction("Fixtures");
            }
        }

        public async Task<IActionResult> Fixtures()
        {
            try
            {
                var fixtures = await this.fixturesService.GetAllInNextGameweek();

                return this.View(fixtures);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["Message"] = ex.Message;
                return this.RedirectToAction("Results");
            }
        }
    }
}
