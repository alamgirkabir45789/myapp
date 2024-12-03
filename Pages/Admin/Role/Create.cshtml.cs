using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.IO;
using System.Threading.Tasks;
using System;
using myportfolio.ViewModels;
using myportfolio.Model.Entity.User;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notyfService;

        [BindProperty]
        public Signup Model { get; set; }

        public CreateModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,INotyfService notyfService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _notyfService = notyfService;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public Model.Entity.Role.UserRole RoleRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            

            

            var role = new Model.Entity.Role.UserRole()
            {
                Name= RoleRequest.RoleName,
            };

          var result=  await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                //await signInManager.SignInAsync(user, false);
                _notyfService.Success("Role has been created successfully!!!");
                return Redirect("/Admin/Role/Index");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToPage("./Index");
        }

    }
}
