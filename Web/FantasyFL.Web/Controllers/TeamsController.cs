namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : Controller
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        public async Task<IActionResult> All()
        {
            var teams = await this.teamsService.GetAll();

            return this.View(teams);
        }

        public async Task<IActionResult> Players(int id)
        {
            var teamPlayers = await this.teamsService.GetTeamPlayers(id);

            return this.View(teamPlayers);
        }
    }
}
