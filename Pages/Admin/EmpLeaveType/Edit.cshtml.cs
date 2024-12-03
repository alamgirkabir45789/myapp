using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model.Entity.User;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.EmpLeaveType
{
    [Authorize(Roles = "Admin")]
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
        public EmpLeaveTypeRequest EmpLeaveRequest { get; set; }

        public async Task OnGet(int? id)
        {

            var url = _config.GetSection("baseUrl").Value;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url + $"api/EmpLeaveType/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    EmpLeaveRequest = JsonConvert.DeserializeObject<EmpLeaveTypeRequest>(json);
                    //DailyReportRequest.TargetDate.ToString("dd MMMM yyyy");
                }

            }


        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var request = new EmpLeaveTypeRequest()
                {
                    LeaveTypeName = EmpLeaveRequest.LeaveTypeName,
                    Description = EmpLeaveRequest.Description,
                    TotalDay = EmpLeaveRequest.TotalDay,

                    Key = EmpLeaveRequest.Key,
                    Id = EmpLeaveRequest.Id,
                    CreatedAt = EmpLeaveRequest.CreatedAt,
                    CreatedBy = EmpLeaveRequest.CreatedBy,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url + $"api/EmpLeaveType/UpdateEmpLeaveType", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Leave type has been updated successfully!!!");
                    return RedirectToPage("./Index");
                }

            }
            return Page();
        }
    }
}
