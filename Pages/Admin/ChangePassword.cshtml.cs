using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.User;
using myportfolio.ViewModels;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly INotyfService _notyfService;

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notyfService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _notyfService = notyfService;
        }
        [BindProperty]
        public ChangePasswordViewModel Model { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user=await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return   Redirect("/Admin");
                }
                var changePasswordView = new ChangePasswordViewModel()
                {
                  CurrentPassword=Model.CurrentPassword,
                  NewPassword=Model.NewPassword,
                  ConfirmPassword=Model.ConfirmPassword,
                };
                var result = await userManager.ChangePasswordAsync(user, changePasswordView.CurrentPassword, changePasswordView.NewPassword);
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    _notyfService.Success("Password has been updated successfully!!!");

                    await signInManager.RefreshSignInAsync(user);
                    return Redirect("/Admin");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("ChangePassword", "OnGet");
        }
    }
}
