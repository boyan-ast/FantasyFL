namespace FantasyFL.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class TransfersController : Controller
    {
        private readonly ITransfersService transfersService;
        private readonly IFantasyTeamsService fantasyTeamService;

        public TransfersController(
            ITransfersService transfersService,
            IFantasyTeamsService fantasyTeamService)
        {
            this.transfersService = transfersService;
            this.fantasyTeamService = fantasyTeamService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await this.fantasyTeamService.UserTeamIsEmpty(userId))
            {
                return this.Redirect("/PlayersManagement/PickGoalkeepers");
            }


            var model = await this.transfersService.GetTransfersList(userId);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> RemovePlayer(int removedPlayerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.transfersService.RemovePlayer(userId, removedPlayerId);

            this.TempData["removedPlayerId"] = removedPlayerId;

            return this.RedirectToAction("PickNewPlayer");
        }

        [Authorize]
        public async Task<IActionResult> PickNewPlayer()
        {
            // TODO: Not open it if the team is full
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            int removedPlayerId;

            removedPlayerId = (int)this.TempData["removedPlayerId"];
            this.TempData.Clear();

            var players = await this.transfersService.GetPlayersToTransfer(userId, removedPlayerId);

            return this.View(players);
        }

        [Authorize]
        public async Task<IActionResult> AddPlayer(int playerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.transfersService.AddPlayer(userId, playerId);

            return this.Redirect("/User/Team");
        }
    }
}
