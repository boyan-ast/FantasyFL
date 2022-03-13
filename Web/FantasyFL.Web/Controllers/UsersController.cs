namespace FantasyFL.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        public async Task<IActionResult> MyTeam()
        {
            return this.View();
        }
    }
}
