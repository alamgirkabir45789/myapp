using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using myportfolio.Model.Entity.User;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;

using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using myportfolio.Pages.Admin.DailyTask;
using myportfolio.Model.Entity.Holiday;
using Azure;
using Newtonsoft.Json;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Pages.Admin.EmpLeaveType;
using myportfolio.Pages.Admin.LeaveRegister;
using System.Security.Cryptography;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class AssignLeaveModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        public AssignLeaveModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;
        }

        [BindProperty]
        public ManageLeaveRequest manageLeaveRequest { get; set; }
        [BindProperty]
        public EmpLeaveRequest empLeaveRequest { get; set; }
        public IList<EmpLeaveTypeRequest> EmpLeaveTypes { get; set; }
        [BindProperty]
        public List<AssignHolidayRequest> assignHolidayRequests { get; set; }
        public IList<EmpLeaveRequest> EmpLeaveRequests { get; set; }
        public async Task<IActionResult> OnGet()
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

            var url = _config.GetSection("baseUrl").Value;
            //var totalLeaveInCurrentYear = 0;
            using (var client = new HttpClient())
            {
                var leaveTypesResponse = await client.GetAsync(url + $"api/Common/GetAllLeaveType");
                if (leaveTypesResponse.IsSuccessStatusCode)
                {
                    var json = await leaveTypesResponse.Content.ReadAsStringAsync();

                    EmpLeaveTypes = JsonConvert.DeserializeObject<IList<EmpLeaveTypeRequest>>(json);
                }
                               
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (empLeaveRequest.LeaveTypeId == 0)
            {
                _notyfService.Error("Something wen wrong!!!");
                return RedirectToPage("AssignLeave", new { id = empLeaveRequest.UserId.ToString() });
            }

            List<EmpLeaveRequest> empReq = new List<EmpLeaveRequest>();
            for (int i = 0; i < assignHolidayRequests.Count; i++)
            {
                var emp = assignHolidayRequests[i];
                if (emp.IsSelected)
                {
                    var request = new EmpLeaveRequest()
                    {

                        LeaveType = empLeaveRequest.LeaveType,
                        LeaveTypeId = empLeaveRequest.LeaveTypeId,
                        UserId = emp.UserId,

                        TotalLeave = empLeaveRequest.TotalLeave,
                        ValidFromDate = empLeaveRequest.ValidFromDate,
                        ValidToDate = empLeaveRequest.ValidToDate,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Email
                    };
                    empReq.Add(request);
                }
            }
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var json = System.Text.Json.JsonSerializer.Serialize(empReq);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await client.PostAsync(url + $"api/EmpLeave/AssignLeaveToMultipleUser", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Leave has been assign successfully!!!");

                    return RedirectToPage("AssignLeave", "OnGet");
                }
                else
                {
                    _notyfService.Error("Something went wrong!!!");

                    return RedirectToPage("AssignLeave", "OnGet");
                }
            }
           
        }
    }
}
