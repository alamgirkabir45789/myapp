using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model;
using myportfolio.Pages.Admin.Role;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class RetriveUserModel : PageModel
    {
        private readonly UserManager<Model.Entity.User.ApplicationUser> userManager;
        private readonly SignInManager<Model.Entity.User.ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private readonly INotyfService _notyfService;


        public RetriveUserModel(ApplicationDbContext context, UserManager<Model.Entity.User.ApplicationUser> userManager, SignInManager<Model.Entity.User.ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _notyfService = notyfService;
        }


        public Model.Entity.User.ApplicationUser ApplicationUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            ApplicationUsers = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            user.isActive = true;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _notyfService.Success("User has been retrive successfully!!!");
                return Redirect("./Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Delete", new { id = id.ToString() });
        }
    }
}
