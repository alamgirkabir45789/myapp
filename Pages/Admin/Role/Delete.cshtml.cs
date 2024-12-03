using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notyfService;
        public DeleteModel(myportfolio.Model.ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration config,INotyfService notyfService)
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;
            _notyfService = notyfService;

        }
        [BindProperty]
        public UserRoleRequest RoleRequest { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x=>x.Id==id);
            if (role != null)
            {
                var singleRole = new UserRoleRequest()
                {
                    Name = role.Name
                };
                RoleRequest = singleRole;
                ViewData["roleId"] = role.Id;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    _notyfService.Success("Role has been deleted successfully!!!");
                    return Redirect("./Index");
                }

            }

            return Page();
        }
    }
}
