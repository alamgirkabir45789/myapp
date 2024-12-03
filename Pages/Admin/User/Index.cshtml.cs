using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyTask;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;

        }


        //public IList<Model.Entity.User.ApplicationUser> ApplicationUsers { get; set; }

        public IList<UserRoleViewModel> UserRoles { get; set; }

        public  IActionResult OnGetAsync()
        {
            //var users = await _context.Users.ToListAsync();
            var users = _userManager.Users.Select(c => new UserRoleViewModel()
            {
                UserId = c.Id,
                ContactNo = c.ContactNo,
                Username = c.Name,
                Email = c.Email,
                EmpCode=c.EmployeeCode,
                IsActive=c.isActive,
                Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();

            UserRoles=users;
            return Page();
        }
    }
}
