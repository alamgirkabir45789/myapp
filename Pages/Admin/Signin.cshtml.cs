using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.LeaveRegister;
using myportfolio.ViewModels;

namespace myportfolio.Pages.Admin
{
    public class SigninModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public Signin Model { get; set; }

        public SigninModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _config = config;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync("alamgirkabir45789@gmail.com");
                if (user != null)
                {
                    if (user.isActive)
                    {
                        var identityResult = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);
                        if (identityResult.Succeeded)
                        {

                            CookieOptions options = new CookieOptions();
                            options.Expires = DateTime.Now.AddDays(1);
                            _httpContextAccessor.HttpContext.Response.Cookies.Append
                            ("name", user.Name, options);
                            if (returnUrl == null || returnUrl == "/")
                            {
                                return Redirect("/Admin");
                            }
                            else
                            {

                                return Redirect(returnUrl);
                            }

                        }

                        ModelState.AddModelError("", "Email or Password not correct.");
                    }
                }

            }

            return Page();
        }
    }
}
