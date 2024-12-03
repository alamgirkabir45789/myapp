using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.DailyTask;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using myportfolio.Model.Entity.User;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using System.Linq;
using myportfolio.Pages.Admin.EmpLeaveType;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class LeaveRecordModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public LeaveRecordModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;

        }
        public IList<LeaveRegisterRequest> LeaveRegisterRequests { get; set; }        
        
        public IList<LeaveSummaryWithLeaveTypeViewModel> LeaveSummaryViewModels { get; set; }
        public IList<EmpLeaveTypeRequest> EmpLeaveTypeReq { get; set; }
        public async Task OnGet( string qparams)
        {
            string input = qparams;
            string cleanedInput = input.Remove(startIndex: 0, count: 5);
           
           
            DateTime formatedDate;
            bool isSuccess = DateTime.TryParse(cleanedInput, out formatedDate);
            if (isSuccess)
            {
                formatedDate.ToString("yyyy-MM-dd");
                using (var client = new HttpClient())
                {
                    var url = _config.GetSection("baseUrl").Value;
                    var response = await client.GetAsync(url + $"api/LeaveRegister/GetLeaveRecordByDate?date={formatedDate}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        LeaveRegisterRequests = JsonConvert.DeserializeObject<IList<LeaveRegisterRequest>>(json);

                    }

                }
            }
            else
            {
                // var user = await _userManager.GetUserAsync(User);

                //var data=await _context.DailyTasks.Include(x=>x.DailyReport).ToListAsync();

                using (var client = new HttpClient())
                {
                    var url = _config.GetSection("baseUrl").Value;
                    var response = await client.GetAsync(url + $"api/EmpLeaveType/GetAll");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        EmpLeaveTypeReq = JsonConvert.DeserializeObject<IList<EmpLeaveTypeRequest>>(json);

                    }
                }

                using (var client = new HttpClient())
                {
                    var url = _config.GetSection("baseUrl").Value;
                    var response = await client.GetAsync(url + $"api/LeaveRegister/GetCurrentYearLeaveRecord?userId={cleanedInput}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        LeaveRegisterRequests = JsonConvert.DeserializeObject<IList<LeaveRegisterRequest>>(json);

                    }
                }

                using (var client = new HttpClient())
                {
                    var url = _config.GetSection("baseUrl").Value;
                    var response = await client.GetAsync(url + $"api/LeaveRegister/GetCurrentYearLeaveSummary?userId={cleanedInput}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        LeaveSummaryViewModels = JsonConvert.DeserializeObject<IList<LeaveSummaryWithLeaveTypeViewModel>>(json);
                        //YearlyLeaveRecords = JsonConvert.DeserializeObject<YearlyLeaveRecord>(json);
                        

                    }
                }
                var user = _context.Users.FirstOrDefault(x => x.Id == cleanedInput);
                ViewData["selectedEmp"] = user.Name;
            }
           
          
        }
    }
}
