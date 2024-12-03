using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.GroupRecources;
using myportfolio.Model.Entity.Resources;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.DailyReport
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        [BindProperty]
        public DailyReportRequest DailyReportRequest { get; set; }

        public async Task OnGet(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var response = await client.GetAsync(url+$"api/DailyReport/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    DailyReportRequest = JsonConvert.DeserializeObject<DailyReportRequest>(json);
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
                var request = new Model.Entity.DailyReport.DailyReport()
                {
                    Name = DailyReportRequest.Name,
                    ProjectName = DailyReportRequest.ProjectName,
                    AssignDate = DailyReportRequest.AssignDate,
                    TargetDate = DailyReportRequest.TargetDate,
                    EndDate = DailyReportRequest.EndDate,
                    IsCompleted = DailyReportRequest.IsCompleted,
                    Key = DailyReportRequest.Key,
                    Id = DailyReportRequest.Id,
                    CreatedAt = DailyReportRequest.CreatedAt,
                    CreatedBy = DailyReportRequest.CreatedBy,
                    UpdatedAt = DateTime.UtcNow,
                UpdatedBy = user.Email
            };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url+$"api/DailyReport/UpdateDailyReport", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToPage("./Index");
                }

            }
            return Page();
        }

       
    }
}
