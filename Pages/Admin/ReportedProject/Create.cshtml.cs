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
using Microsoft.Extensions.Configuration;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyReport;

namespace myportfolio.Pages.Admin.ReportedProject
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
            _notyfService = notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ReportedProjectRequested ReportedProjectRequested { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var request = new ReportedProjectRequested()
                {
                    ProjectName = ReportedProjectRequested.ProjectName,
                    Description = ReportedProjectRequested.Description,
                    IsCompleted = false,
              
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url + $"api/ReportedProject/CreateReportedProject", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Project has been submitted successfully!!!");
                    return RedirectToPage("./Index");
                }

            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Create", "OnGet");
        }
    }
}
