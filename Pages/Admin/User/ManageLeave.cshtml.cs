using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.EmpLeaveType;
using myportfolio.Pages.Admin.LeaveRegister;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class ManageLeaveModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;

        public ManageLeaveModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;

        }

        [BindProperty]
        public EmpLeaveRequest empLeaveRequest { get; set; }
        public IList<EmpLeaveTypeRequest> EmpLeaveTypes { get; set; }
        public IList<LeaveSummaryWithLeaveTypeViewModel> LeaveSummaryViewModels { get; set; }
        public IList<EmpLeaveRequest> EmpLeaveRequests { get; set; }
        public async Task OnGet(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            ViewData["userId"] = user.Id;
            ViewData["Name"] = user.Name;
            ViewData["Email"] = user.Email;

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

                var leaveResponse = await client.GetAsync(url + $"api/EmpLeave/GetAllLeaveByUserId?userId={id}");
                if (leaveResponse.IsSuccessStatusCode)
                {
                    var json = await leaveResponse.Content.ReadAsStringAsync();
                    EmpLeaveRequests = JsonConvert.DeserializeObject<IList<EmpLeaveRequest>>(json);
                }

                var response = await client.GetAsync(url + $"api/LeaveRegister/GetCurrentYearLeaveSummary?userId={user.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    LeaveSummaryViewModels = JsonConvert.DeserializeObject<IList<LeaveSummaryWithLeaveTypeViewModel>>(json);
                }

            }
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            var user = await _userManager.GetUserAsync(User);
            if(empLeaveRequest.LeaveTypeId == 0)
            {
                _notyfService.Error("Something wen wrong!!!");
                return RedirectToPage("ManageLeave", new { id = empLeaveRequest.UserId.ToString() });
            }

            using (var client = new HttpClient())
            {
                var baseUrl = _config.GetSection("baseUrl").Value; 

                if (empLeaveRequest.Id > 0)
                {
                    var request = new EmpLeaveRequest()
                    {

                        LeaveType = empLeaveRequest.LeaveType,
                        LeaveTypeId = empLeaveRequest.LeaveTypeId,
                        UserId = empLeaveRequest?.UserId,

                        TotalLeave = empLeaveRequest.TotalLeave,
                        ValidFromDate = empLeaveRequest.ValidFromDate,
                        ValidToDate = empLeaveRequest.ValidToDate,
                        Key = empLeaveRequest.Key,
                        Id = empLeaveRequest.Id,
                        UpdatedAt = DateTime.UtcNow,
                        UpdatedBy = user.Email
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(baseUrl + $"api/EmpLeave/UpdateEmpLeave", content);
                    var resjson = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        _notyfService.Success("Leave has been updated successfully!!!");
                        return RedirectToPage("ManageLeave", new { id = empLeaveRequest.UserId.ToString() });
                    }
                }
                else
                {
                    var request = new EmpLeaveRequest()
                    {

                        LeaveType = empLeaveRequest.LeaveType,
                        LeaveTypeId = empLeaveRequest.LeaveTypeId,
                        UserId = empLeaveRequest?.UserId,
                        TotalLeave = empLeaveRequest.TotalLeave,
                        ValidFromDate = empLeaveRequest.ValidFromDate,
                        ValidToDate = empLeaveRequest.ValidToDate,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Email
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(baseUrl + $"api/EmpLeave/CreateEmpLeave", content);
                    var resjson = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        _notyfService.Success("Leave has been assign successfully!!!");
                        return RedirectToPage("ManageLeave", new { id = empLeaveRequest.UserId.ToString() });
                    }
                }



            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("ManageLeave", new { id = empLeaveRequest.UserId.ToString() });
        }

    }
}
