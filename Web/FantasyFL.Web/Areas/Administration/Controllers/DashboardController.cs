namespace FantasyFL.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IGameweeksService gameweeksService;

        public DashboardController(IGameweeksService gameweeksService)
        {
            this.gameweeksService = gameweeksService;
        }

        public async Task<IActionResult> Index()
        {
            var gameweeks = await this.gameweeksService.GetAllAsync();

            return this.View(gameweeks);
        }

        public async Task<IActionResult> GetData(int id)
        {
            await this.gameweeksService.GetPlayersData(id);

            return this.Redirect("/Administration/Dashboard");
        }

        public async Task<IActionResult> Finish(int id)
        {
            await this.gameweeksService.FinishGameweek(id);

            return this.Redirect("/Administration/Dashboard");
        }
    }
}
