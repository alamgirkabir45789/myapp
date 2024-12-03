using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyTask;

namespace myportfolio.Pages.Admin.Report
{
    [Authorize(Roles = "Admin")]
    public class MonthlyTaskReportModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public MonthlyTaskReportModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;

        }
     
        public IList<TaskReportViewModel> TaskReports { get; set; }
        public async Task<IActionResult> OnGet(string userId,DateTime date)
        {
            var user = await _userManager.GetUserAsync(User);
          
            var userList = await (from p in _context.Users
                                  join ps in _context.UserRoles on p.Id equals ps.UserId
                                  join rs in _context.Roles on ps.RoleId equals rs.Id
                                  where rs.Name == "Employee"
                                  select new
                                  {
                                      p.Id,
                                      p.Name,
                                      p.Email,
                                      p.ContactNo,
                                      p.EmployeeCode
                                  }
                          ).ToListAsync();
     
            ViewData["users"] = userList.Select(s => new SelectListItem
            {
               Value = s.Id.ToString(),
                Text = s.EmployeeCode + " " + s.Name
            });
            
         
            return Page();
        }
    }
}
