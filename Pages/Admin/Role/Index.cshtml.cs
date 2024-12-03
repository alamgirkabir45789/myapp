using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(myportfolio.Model.ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;

        }
        
        public IList<UserRoleRequest> UserRoles { get; set; }
        public IActionResult OnGetAsync()
        {
            var roleList =_roleManager.Roles.ToList();
            ViewData["RoleList"] = roleList;
            return Page();
        }
    }
}
