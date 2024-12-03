using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.EmpLeaveType;
using myportfolio.Pages.Admin.LeaveRegister;
using myportfolio.Pages.Admin.Role;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }


        public Model.Entity.User.ApplicationUser ApplicationUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
          
            ApplicationUsers = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            ViewData["userId"]= id;
            return Page();
            
        }
    }
}
