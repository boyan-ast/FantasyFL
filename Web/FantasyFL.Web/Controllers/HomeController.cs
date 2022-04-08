namespace FantasyFL.Web.Controllers
{
    using System.Diagnostics;

    using FantasyFL.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    using static FantasyFL.Common.GameweeksData;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            return this.Redirect("/UserTeam");
        }

        public IActionResult Rules()
        {
            return this.View(GameweeksDeadlines);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
