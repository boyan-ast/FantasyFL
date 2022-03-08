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
    using Newtonsoft.Json;

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

            var pickGoalkeepersModel = new PickPlayersFormModel
            {
                Players = allPlayers,
            };

            if (this.TempData["players"] != null)
            {
                this.TempData["players"] = JsonConvert.DeserializeObject<PickPlayersFormModel>(this.TempData["players"].ToString());
                pickGoalkeepersModel = this.TempData["players"] as PickPlayersFormModel;
                pickGoalkeepersModel.Players = allPlayers;
                this.TempData.Remove("players");
            }

            return this.View(pickGoalkeepersModel);
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

            // The tempdate is in case of invalid players list
            this.TempData["players"] = JsonConvert.SerializeObject(model);

            return this.RedirectToAction("PickGoalkeepers");
        }
    }
}
