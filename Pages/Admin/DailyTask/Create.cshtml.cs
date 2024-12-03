using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.ReportedProject;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyReport;

namespace myportfolio.Pages.Admin.DailyTask
{
   [Authorize(Roles = "Employee,Developer,Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,IConfiguration config,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var DailyReportRequests = await _context.ReportedProjects.ToListAsync();
            var userList= await  (from p in _context.Users.Where(x=>x.isActive)
                                               join ps in _context.UserRoles on p.Id equals ps.UserId
                                               join rs in _context.Roles on ps.RoleId equals rs.Id
                                               where rs.Name == "Employee"
                                               select new
                                               {
                                                   p.Id,
                                                   p.Name,
                                                   p.Email,
                                                   p.ContactNo
                                               }
                          ).ToListAsync();
            
            ViewData["ReportedProject"] = new SelectList(DailyReportRequests, "Id", "ProjectName");
            ViewData["users"] = new SelectList(userList, "Name", "Name");
           
            ViewData["user"] = user.Name;
            return Page();
        }

        [BindProperty]
        public DailyTaskReques DailyTaskReques { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Warning("Please provide all informations!!!");
                return RedirectToPage("Create", "OnGet");
            }
            var user = await _userManager.GetUserAsync(User);
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == DailyTaskReques.Name);
                string project = DailyTaskReques.WorkingProject;
                string[] projects = project.Split(',');
               
                for (int i = 0; i < projects.Length; i++)
                {
                    string pjName = (projects[i]);
                    if (pjName != null)
                    {
                        var prName = _context.ReportedProjects.FirstOrDefault(x => x.Id.ToString() == pjName);
                        if (prName == null)
                        {
                            _notyfService.Error("Invalid project name,please select valid project name!!!");
                            return RedirectToPage("Create", "OnGet");
                        }
                    }
                }

               
                var userId = await _userManager.FindByIdAsync(selectedUser.Id);
                var request = new DailyTaskReques()
                {
                    Name = DailyTaskReques.Name,
                    UserId=userId.Id,
                    Description = DailyTaskReques.Description,
                    SubmitDate = DailyTaskReques.SubmitDate,
                    InTime = DailyTaskReques.InTime,
                    OutTime = DailyTaskReques.OutTime,
                    WorkingProject = DailyTaskReques.WorkingProject,
                    IsHoliday= DailyTaskReques.IsHoliday,
         
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url+$"api/DailyTask/CreateDailyTask", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Task has been submitted successfully!!!");
                    return RedirectToPage("./Index");
                }
                else
                {
                    _notyfService.Error(resjson);
                    return RedirectToPage("Create", "OnGet");
                }

            }
            
        }
    }
}
