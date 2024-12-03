using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using myportfolio.Model.Entity.User;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using myportfolio.Pages.Admin.DailyTask;
using myportfolio.Model.Entity.Holiday;
using Azure;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class AssignHolidayModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        public AssignHolidayModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;
        }

        [BindProperty]
        public DailyTaskReques DailyTaskReques { get; set; }
        [BindProperty]
        public List<AssignHolidayRequest> assignHolidayRequests { get; set; }

     
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var DailyReportRequests = await _context.ReportedProjects.ToListAsync();
            var userList = await (from p in _context.Users
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
            assignHolidayRequests = new List<AssignHolidayRequest>();
            foreach (var item in userList)
            {
                var assignHDay = new AssignHolidayRequest()
                {
                    UserId = item.Id,
                    UserName = item.Name,
                    IsSelected = false
                };
            assignHolidayRequests.Add(assignHDay);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool holidayStatus)
        {
            var user = await _userManager.GetUserAsync(User);           

            List<DailyTaskReques> dsk = new List<DailyTaskReques>();
            for (int i = 0; i < assignHolidayRequests.Count ; i++)
            {
                var ulist =assignHolidayRequests[i];
                if(ulist != null)
                {
                    if (ulist.IsSelected)
                    {
                        var request = new DailyTaskReques()
                        {
                            Name = ulist.UserName,
                            UserId = ulist.UserId,
                            Description = DailyTaskReques.Description,
                            SubmitDate = DailyTaskReques.SubmitDate,
                            InTime = DailyTaskReques.InTime,
                            OutTime = DailyTaskReques.OutTime,
                            WorkingProject = "",
                            IsHoliday = true,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = user.Email
                        };
                        dsk.Add(request);
                    }
                }
                          
            }
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var json2 = System.Text.Json.JsonSerializer.Serialize(dsk);
                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");


                var response2 = await client.PostAsync(url + $"api/DailyTask/CreateOrUpdateMultiDailyTask", content2);
                var resjson2 = await response2.Content.ReadAsStringAsync();
                if (response2.IsSuccessStatusCode)
                {
                    _notyfService.Success("Data has been submitted successfully!!!");

                    return RedirectToPage("AssignHoliday", "OnGet");
                }
                else
                {
                    _notyfService.Error("Something went wrong!!!");

                    return RedirectToPage("AssignHoliday", "OnGet");
                }
            }
           
        }
    }
}
