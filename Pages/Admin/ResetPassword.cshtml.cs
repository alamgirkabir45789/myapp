using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model;
using myportfolio.ViewModels;
using myportfolio.Model.Entity.User;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public ResetPasswordModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ResetPasswordViewModel ResetPasswordViewModels { get; set; }

        public IActionResult OnGet([FromQuery] string code, [FromQuery] string email)
        {
            ResetPasswordViewModel rsp = new ResetPasswordViewModel()
            {
                Email = email,
                Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            };
            ResetPasswordViewModels = rsp;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string token)
        {
            if (ModelState.IsValid)
            {
                var resetPasword = new ResetPasswordViewModel()
                {
                    Password = ResetPasswordViewModels.Password,
                    Token =  ResetPasswordViewModels.Token,
                    Email = ResetPasswordViewModels.Email
                };
                var user = await userManager.FindByEmailAsync(resetPasword.Email);
                if (user != null )
                {
                    var result = await userManager.ResetPasswordAsync(user, resetPasword.Token, resetPasword.Password);
                    if (result.Succeeded)
                    {
                        return Redirect("/Admin");
                    }
                    return Page();
                }
               
            }
            return Page();
        }
    }
}
