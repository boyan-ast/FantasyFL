namespace FantasyFL.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class TransfersController : Controller
    {
        private readonly ITransfersService transfersService;
        private readonly IFantasyTeamsService fantasyTeamService;
        private readonly IPlayersService playersService;

        public TransfersController(
            ITransfersService transfersService,
            IFantasyTeamsService fantasyTeamService,
            IPlayersService playersService)
        {
            this.transfersService = transfersService;
            this.fantasyTeamService = fantasyTeamService;
            this.playersService = playersService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await this.fantasyTeamService.UserTeamIsEmpty(userId))
            {
                return this.Redirect("/PlayersManagement/PickGoalkeepers");
            }

            try
            {
                var model = await this.transfersService.GetTransfersList(userId);

                return this.View(model);
            }
            catch (InvalidOperationException ex)
            {
                this.TempData["Message"] = ex.Message;
                return this.RedirectToAction("Index", "UserTeam");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PickNewPlayer(int removedPlayerId)
        {
            // TODO: Not open it if the team is full
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var playerPosition = await this.playersService.GetPlayerPosition(removedPlayerId);

            await this.transfersService.RemovePlayer(userId, removedPlayerId);

            var players = await this.transfersService.GetPlayersToTransfer(userId, playerPosition);

            return this.View(players);
        }

        [Authorize]
        public async Task<IActionResult> AddPlayer(int playerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.transfersService.AddPlayer(userId, playerId);

            return this.Redirect("/UserTeam/Index");
        }
    }
}
