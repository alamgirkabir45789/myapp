using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.User;
using myportfolio.ViewModels;

namespace myportfolio.Pages.Admin
{
    //[Authorize(Roles = "Admin")]
    public class SignupModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public Signup Model { get; set; }

        public SignupModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name=Model.Name,
                    ContactNo = Model.ContactNo,
                    UserName = Model.Email,
                    Email = Model.Email,
                    EmployeeCode = Model.EmployeeCode,
                    EmailConfirmed=true
                };
                var result = await userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    return Redirect("/Admin/User/Index");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}
