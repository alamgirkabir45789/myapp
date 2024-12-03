using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.Role;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class ManageRoleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;

        public ManageRoleModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,INotyfService notyfService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _notyfService = notyfService;
        }
        [BindProperty]
        public List<AssignRoleRequest> AssignRoleRequests { get; set; }
        public async Task OnGet(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            ViewData["userId"] = user.Id;
            ViewData["Name"] = user.Name;
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

        }



        public async Task<IActionResult> OnPostAsync(List<AssignRoleRequest> roleRequests, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
           
            int numCount = 0;
            if(AssignRoleRequests.Count > 0)
            {
             
                for (int i = 0; i < AssignRoleRequests.Count; i++)
                {
                    var roleRequest = AssignRoleRequests[i];
                    if(roleRequest.IsSelected && roleRequest.RoleName == "Admin")
                    {
                        numCount = numCount + 1;
                    }
                    else if (roleRequest.IsSelected && roleRequest.RoleName == "Employee")
                    {
                        numCount = numCount + 1;
                    }
                }
            }
            if(numCount ==2)
            {
                _notyfService.Warning("You can not select admin and Employee role!!!");
                return RedirectToPage("ManageRole", new { id = userId });
            }
            //bool isAdminDev = false;
            //if(AssignRoleRequests.Count > 0)
            //{
             
            //    for (int i = 0; i < AssignRoleRequests.Count; i++)
            //    {
            //        var roleRequest = AssignRoleRequests[i];
            //        if(roleRequest.IsSelected && roleRequest.RoleName == "Admin")
            //        {
            //            isAdminDev = true;
            //        }
            //        else if (roleRequest.IsSelected && roleRequest.RoleName == "Employee")
            //        {
            //            isAdminDev = true;
            //        }
            //    }
            //}
            //if(isAdminDev)
            //{
            //    _notyfService.Warning("You can not select admin and Employee role!!!");
            //    return RedirectToPage("ManageRole", new { id = userId });
            //}
            for (int i = 0; i < AssignRoleRequests.Count; i++)
            {
                 var role = await _roleManager.FindByIdAsync(AssignRoleRequests[i].RoleId);
                    //var role = await _roleManager.FindByIdAsync(AssignRoleRequests[i].RoleId);

                    IdentityResult result = null;

                    if (AssignRoleRequests[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    if (!AssignRoleRequests[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if (i < (AssignRoleRequests.Count - 1))
                            continue;
                        
                    }
                
               
            }


            _notyfService.Success("Role has been assigned successfully!!!");
            return RedirectToPage("./Index");
        }

    }
}
