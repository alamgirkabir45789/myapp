using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyReport;
using myportfolio.Pages.Admin.DailyTask;
using myportfolio.Pages.Admin.EmpLeaveType;
using myportfolio.Pages.Admin.User;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;

        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;

        }
        public YearlyLeaveRecord YearlyLeaveRecords { get; set; }
        //public IList<LeaveSummaryWithLeaveTypeViewModel> LeaveSummaryViewModels { get; set; }
        public IList<EmpLeaveTypeRequest> EmpLeaveTypeReq { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            var url = _config.GetSection("baseUrl").Value;
            //var totalLeaveInCurrentYear = 0;
            using (var client = new HttpClient())
            {
                var leaveTypesResponse = await client.GetAsync(url + $"api/EmpLeaveType/GetAll");
                if (leaveTypesResponse.IsSuccessStatusCode)
                {
                    var json = await leaveTypesResponse.Content.ReadAsStringAsync();
                    EmpLeaveTypeReq = JsonConvert.DeserializeObject<IList<EmpLeaveTypeRequest>>(json);
                   
                }
                //var response = await client.GetAsync(url + $"api/LeaveRegister/GetCurrentYearLeaveSummary?userId={user.Id}");
                //if (response.IsSuccessStatusCode)
                //{
                //    var json = await response.Content.ReadAsStringAsync();
                //    //YearlyLeaveRecords = JsonConvert.DeserializeObject<YearlyLeaveRecord>(json);
                //    LeaveSummaryViewModels = JsonConvert.DeserializeObject<IList<LeaveSummaryWithLeaveTypeViewModel>>(json);
                //}

            }

            var designation = await _context.Teams.FirstOrDefaultAsync(x => x.UserId == user.Id);
            ViewData["Name"] = user.Name;
            ViewData["Email"] = user.Email;
            ViewData["ContactNo"] = user.ContactNo;
            ViewData["Designation"] = designation == null ? "" : designation.Designation;

            var list = await _context.Users.Where(x=>x.isActive).ToListAsync();
            ViewData["UserList"] = new SelectList(list, "Id", "Name");

            ViewData["UserId"] = user.Id;
            //ViewData["CurrentYearLeave"] = totalLeaveInCurrentYear;
            ViewData["BaseUrl"] = url;
            return Page();
        }

        [BindProperty]
        public LeaveRegisterRequest LeaveRegisterRequest { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}


            var user = await _userManager.GetUserAsync(User);
            
            using (var client = new HttpClient())
            {
                var baseUrl = _config.GetSection("baseUrl").Value;
                double day = 0;
                if (LeaveRegisterRequest.IsDays)
                {

                    day = Math.Abs((LeaveRegisterRequest.ToDate.Date - LeaveRegisterRequest.FromDate.Date).Days + 1);

                }
                if(LeaveRegisterRequest.IsHalfDay)
                {
                    day = LeaveRegisterRequest.TotalDay;
                }
                var userId="";
                if (User.IsInRole("Admin"))
                {
                    userId=LeaveRegisterRequest.UserId;
                }
                else
                {
                    userId=user.Id;
                }
                var request = new LeaveRegisterRequest()
                {

                    Name = LeaveRegisterRequest.Name,
                    Email = LeaveRegisterRequest.Email,
                    UserId = userId,
                    ContactNo = LeaveRegisterRequest.ContactNo,
                    Designation = LeaveRegisterRequest.Designation,
                    Reason = LeaveRegisterRequest.Reason,
                    LeaveType = LeaveRegisterRequest.LeaveType,
                    IsHours = LeaveRegisterRequest.IsHours,
                    IsDays = LeaveRegisterRequest.IsDays,
                    IsHalfDay = LeaveRegisterRequest.IsHalfDay,
                    FromDate = LeaveRegisterRequest.FromDate,
                    ToDate = LeaveRegisterRequest.ToDate,
                    HourlyLeaveDate = LeaveRegisterRequest.HourlyLeaveDate,
                    ApprovedFromDate = LeaveRegisterRequest.FromDate,
                    ApprovedToDate = LeaveRegisterRequest.ToDate,
                    FromTime = LeaveRegisterRequest.FromTime,

                    //FromTime = LeaveRegisterRequest.FromTime.ToString("HH:mm"),
                    ToTime = LeaveRegisterRequest.ToTime,
                    TotalDay = day,
                    ApprovedTotalDay = 0,
                    Comments = LeaveRegisterRequest.Comments,
                    IsAccepted = LeaveRegisterRequest.IsAccepted,
                    IsReject = LeaveRegisterRequest.IsReject,
                    IsChangeDate = false,


                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };
                //if(!request.IsDays && !request.IsHours)
                //{
                //    return Page();
                //};

                if ((LeaveRegisterRequest.IsDays == true) || (LeaveRegisterRequest.IsHours == true) || (LeaveRegisterRequest.IsHalfDay == true))
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(baseUrl + $"api/LeaveRegister/PostLeaveRegister", content);
                    var resjson = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        _notyfService.Success("Your leave application has been submitted successfully!!!");
                        return RedirectToPage("./Index");
                    }
                }
                else
                {
                    _notyfService.Error("Something wen wrong!!!");
                    return RedirectToPage("Create", "OnGet");
                }

            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Create", "OnGet");
        }
    }
}
