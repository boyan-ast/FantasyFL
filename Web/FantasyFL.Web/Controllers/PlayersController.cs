namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class PlayersController : Controller
    {
        private readonly IPlayersService playersService;

        public PlayersController(IPlayersService playersService)
        {
            this.playersService = playersService;
        }

        public async Task<IActionResult> Squad(int id)
        {
            var players = await this.playersService.GetAllByTeam(id);

            return this.View(players);
        }
    }
}
