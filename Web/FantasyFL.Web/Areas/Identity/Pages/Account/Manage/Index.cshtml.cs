namespace FantasyFL.Web.Areas.Identity.Pages.Account.Manage
{
#nullable disable

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using FantasyFL.Data.Models;
    using FantasyFL.Services.Data.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649 // File name should match first type name
    public class IndexModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IGameweeksService gameweeksService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGameweeksService gameweeksService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.gameweeksService = gameweeksService;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public int StartGameweek { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public async Task<IActionResult> OnGetAsync()
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var startGameweek = await this.gameweeksService.GetGameweekNumberById(user.StartGameweekId);

            this.Username = userName;
            this.Email = email;
            this.StartGameweek = startGameweek;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
            };
        }
    }
}
