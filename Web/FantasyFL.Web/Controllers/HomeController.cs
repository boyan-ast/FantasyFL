namespace FantasyFL.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using FantasyFL.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPlayersManagementService playersManagementService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IPlayersManagementService playersManagementService)
        {
            this.userManager = userManager;
            this.playersManagementService = playersManagementService;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Identity/Account/Login");
            }

            var userId = this.userManager.GetUserId(this.User);

            return this.Redirect("/Fantasy/PickTeam");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
