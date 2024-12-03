using System;
using System.Collections.Generic;
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

using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyTask;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;

        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;
        }

        [BindProperty]
        public LeaveRegisterRequest LeaveRegisterRequest { get; set; }
        
        //public LeaveSummaryWithLeaveTypeViewModel leaveSummaryWithLeaveTypeViewModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var url = _config.GetSection("baseUrl").Value;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url+$"api/LeaveRegister/GetLeaveRegisterById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    LeaveRegisterRequest = JsonConvert.DeserializeObject<LeaveRegisterRequest>(json);
                    //DailyReportRequest.TargetDate.ToString("dd MMMM yyyy");
                }
            }

            //var user = await _userManager.GetUserAsync(User);

            
            //double totalLeaveInCurrentYear = 0;
           
            //    using (var client = new HttpClient())
            //    {
            //        var response = await client.GetAsync(url + $"api/LeaveRegister/GetLeaveRecordByLeaveType?userId={LeaveRegisterRequest.UserId}&leaveType={LeaveRegisterRequest.LeaveType}");
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var json = await response.Content.ReadAsStringAsync();
            //            leaveSummaryWithLeaveTypeViewModel = JsonConvert.DeserializeObject<LeaveSummaryWithLeaveTypeViewModel>(json);

            //        }
            //        totalLeaveInCurrentYear = leaveSummaryWithLeaveTypeViewModel.RemainingLeave;
            //    }
            
         
            //ViewData["CurrentYearLeave"] = totalLeaveInCurrentYear;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var approvedFromDate=DateTime.Now;
                var approvedToDate=DateTime.Now;
                double ApprovedDay = 0;
                if (LeaveRegisterRequest.IsChangeDate)
                {
                    approvedFromDate = LeaveRegisterRequest.ApprovedFromDate;
                    approvedToDate = LeaveRegisterRequest.ApprovedToDate;
                    ApprovedDay = Math.Abs((LeaveRegisterRequest.ApprovedToDate.Date - LeaveRegisterRequest.ApprovedFromDate.Date).Days + 1);
                }
                else
                {
                    approvedFromDate = LeaveRegisterRequest.FromDate;
                    approvedToDate = LeaveRegisterRequest.ToDate;
                    ApprovedDay = LeaveRegisterRequest.TotalDay;
                }




                var request = new LeaveRegisterRequest()
                {

                    Name = LeaveRegisterRequest.Name,
                    Email = LeaveRegisterRequest.Email,
                    UserId = user.Id,
                    ContactNo = LeaveRegisterRequest.ContactNo,
                    Designation = LeaveRegisterRequest.Designation,
                    Reason = LeaveRegisterRequest.Reason,
                    LeaveType = LeaveRegisterRequest.LeaveType,
                    IsHours = LeaveRegisterRequest.IsHours,
                    IsDays = LeaveRegisterRequest.IsDays,
                    IsHalfDay=LeaveRegisterRequest.IsHalfDay,
                    FromDate = LeaveRegisterRequest.FromDate,
                    ToDate = LeaveRegisterRequest.ToDate,
                    HourlyLeaveDate=LeaveRegisterRequest.HourlyLeaveDate,
                    FromTime = LeaveRegisterRequest.FromTime,
                    ToTime = LeaveRegisterRequest.ToTime,

                    ApprovedFromDate = approvedFromDate,
                    ApprovedToDate = approvedToDate,

                    TotalDay = LeaveRegisterRequest.TotalDay,
                    ApprovedTotalDay = ApprovedDay,
                    Comments = LeaveRegisterRequest.Comments,
                    IsAccepted = LeaveRegisterRequest.IsAccepted,
                    IsReject = LeaveRegisterRequest.IsReject,
                    IsChangeDate = LeaveRegisterRequest.IsChangeDate,
                    Key = LeaveRegisterRequest.Key,
                    Id = LeaveRegisterRequest.Id,
                    CreatedAt = LeaveRegisterRequest.CreatedAt,
                    CreatedBy = LeaveRegisterRequest.CreatedBy,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url+ $"api/LeaveRegister/PutLeaveRegister", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    if(LeaveRegisterRequest.IsAccepted)
                    {

                    _notyfService.Success("Your leave application has been approved!!!");
                    }else if (LeaveRegisterRequest.IsReject)
                    {
                        _notyfService.Warning("Your leave application has been rejected!!!");

                    }
                    return RedirectToPage("./Index");
                }

            }
            return Page();
        }

       
    }
}
