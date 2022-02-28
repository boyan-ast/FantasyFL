namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class AdminController : Controller
    {
        private readonly IGameweekService gameweeksService;

        public AdminController(IGameweekService gameweeksService)
        {
            this.gameweeksService = gameweeksService;
        }

        public async Task<IActionResult> ShowGameweeks()
        {
            var gameweeks = await this.gameweeksService.GetAllAsync();

            return this.View(gameweeks);
        }
    }
}
