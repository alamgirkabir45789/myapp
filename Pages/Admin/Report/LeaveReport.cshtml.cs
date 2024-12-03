using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using myportfolio.Model.Entity.User;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace myportfolio.Pages.Admin.Report
{
    [Authorize(Roles = "Admin")]
    public class LeaveReportModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveReportModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;

        }
        public async Task<IActionResult> OnGet()
        {
            var userList =await _context.Users.ToListAsync();
            ViewData["users"] = userList.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.EmployeeCode + " " + s.Name
            });
            return Page();
        }
    }
}
