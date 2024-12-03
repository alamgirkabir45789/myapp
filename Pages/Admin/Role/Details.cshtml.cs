using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DetailsModel(myportfolio.Model.ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;

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
            }
            return Page();
        }
    }
}
