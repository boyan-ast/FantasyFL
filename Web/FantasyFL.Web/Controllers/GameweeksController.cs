namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class GameweeksController : Controller
    {
        private readonly IGameweekService gameweeksService;

        public GameweeksController(IGameweekService gameweeksService)
        {
            this.gameweeksService = gameweeksService;
        }

        public async Task<IActionResult> All()
        {
            var gameweeks = await this.gameweeksService.GetAllAsync();

            return this.View(gameweeks);
        }

        public async Task<IActionResult> GetData(int id)
        {
            await this.gameweeksService.GetPlayersData(id);

            return this.Redirect("/Gameweeks/All");
        }

        public async Task<IActionResult> Finish(int id)
        {
            await this.gameweeksService.FinishGameweek(id);

            return this.Redirect("/Gameweeks/All");
        }
    }
}
