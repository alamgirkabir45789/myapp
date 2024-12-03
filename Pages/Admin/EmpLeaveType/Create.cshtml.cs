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
using Microsoft.Extensions.Configuration;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
namespace myportfolio.Pages.Admin.EmpLeaveType
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService= notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EmpLeaveTypeRequest EmpLeaveRequest { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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

                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url + $"api/EmpLeaveType/CreateEmpLeaveType", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Leave Type has been created successfully!!!");
                    return RedirectToPage("./Index");
                }

            }
            return Page();
        }
    }
}
