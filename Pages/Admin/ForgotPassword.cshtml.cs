using Habanero.BO.ClassDefinition;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Identity.Client;
using NuGet.Common;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.ViewModels;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ForgotPasswordViewModel ForgotPasswordViewModels { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var forgotPasword = new ForgotPasswordViewModel()
                {
                    Email = ForgotPasswordViewModels.Email
                };
                var user = await userManager.FindByEmailAsync(forgotPasword.Email);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var code = await userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    //var callbackUrl = Url.Page(
                    //    "./ResetPassword",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", code, forgotPasword.Email },
                    //    protocol: Request.Scheme);

                    var callbackUrl = Url.Page(
                "./ResetPassword",
                    pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);




                    await _emailSender.SendEmailAsync(forgotPasword.Email, "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return Redirect("./ConfirmPassword");
                }


            }



            return Page();
        }
    }
}
