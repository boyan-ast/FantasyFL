namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Data.Models.Enums;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels.FantasyTeam;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FantasyTeamController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFantasyTeamsService fantasyTeamService;
        private readonly IPlayersService playersService;

        public FantasyTeamController(
            UserManager<ApplicationUser> userManager,
            IFantasyTeamsService fantasyTeamService,
            IPlayersService playersService)
        {
            this.userManager = userManager;
            this.fantasyTeamService = fantasyTeamService;
            this.playersService = playersService;
        }

        [Authorize]
        public async Task<IActionResult> PickGoalkeepers()
        {
            var userId = this.userManager.GetUserId(this.User);

            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickGoalKeepersFormModel = new PickPlayersFormModel
            {
                Players = allPlayers,
            };

            return this.View(pickGoalKeepersFormModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickDefenders(PickPlayersFormModel model)
        {
            // TODO: Validate model
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickDefendersFormModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
            };

            return this.View(pickDefendersFormModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickMidfielders(PickPlayersFormModel model)
        {
            // TODO: Validate model
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickMidfieldersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
                Defenders = model.Defenders,
            };

            return this.View(pickMidfieldersModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickAttackers(PickPlayersFormModel model)
        {
            // TODO: Validate model
            var allPlayers = await this.playersService
                .GetAllPlayers();

            var pickAttackersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
                Goalkeepers = model.Goalkeepers,
                Defenders = model.Defenders,
                Midfielders = model.Midfielders,
            };

            return this.View(pickAttackersModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitTeam(PickPlayersFormModel model)
        {
            // TODO: Validate model
            // TODO: Edit if needed

            return this.Redirect("/");
        }
    }
}
