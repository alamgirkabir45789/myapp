using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin
{
    public class SignoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public SignoutModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSignoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("Signin");
        }

        public async Task<IActionResult> OnPostDontSignoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
