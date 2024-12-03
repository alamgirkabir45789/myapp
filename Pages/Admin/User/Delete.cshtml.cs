using System;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.Role;
using myportfolio.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<Model.Entity.User.ApplicationUser> userManager;
        private readonly SignInManager<Model.Entity.User.ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private readonly INotyfService _notyfService;


        public DeleteModel(ApplicationDbContext context, UserManager<Model.Entity.User.ApplicationUser> userManager, SignInManager<Model.Entity.User.ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,INotyfService notyfService)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _notyfService = notyfService;
        }

       
        public Model.Entity.User.ApplicationUser ApplicationUsers { get; set; }
        [BindProperty]
        public List<AssignRoleRequest> AssignRoleRequests { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            ApplicationUsers = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            var user = await userManager.FindByIdAsync(id);
            AssignRoleRequests = new List<AssignRoleRequest>();
            foreach (var item in _roleManager.Roles)
            {
                var roleRequest = new AssignRoleRequest()
                {
                    RoleId = item.Id,
                    RoleName = item.Name,


                };

                if (await userManager.IsInRoleAsync(user, item.Name))
                {
                    roleRequest.IsSelected = true;
                }
                else
                {
                    roleRequest.IsSelected = false;
                }
                AssignRoleRequests.Add(roleRequest);
            }


            return Page();

        }

        public async Task <IActionResult> OnPostAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
           if(AssignRoleRequests.Count>0)
            {
                for (int i = 0; i < AssignRoleRequests.Count; i++)
                {
                    var role = await _roleManager.FindByIdAsync(AssignRoleRequests[i].RoleId);
                    //var role = await _roleManager.FindByIdAsync(AssignRoleRequests[i].RoleId);

                    IdentityResult result1 = null;

                    if (AssignRoleRequests[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result1 = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    if (!AssignRoleRequests[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result1 = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result1.Succeeded)
                    {
                        if (i < (AssignRoleRequests.Count - 1))
                            continue;

                    }


                }
            }


            
            user.isActive = false;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _notyfService.Success("User has been deactivated successfully!!!");
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
