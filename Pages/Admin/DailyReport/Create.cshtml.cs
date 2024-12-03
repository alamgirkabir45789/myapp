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
using myportfolio.Model.Entity.Catalogs;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.Product;
using myportfolio.Pages.Admin.Team;

namespace myportfolio.Pages.Admin.DailyReport
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }
        public IList<Model.Entity.Team.Team> TeamRequest { get; set; }
        //public IList<Team.Request> TeamRequest { get; set; }
        public IList<Model.Entity.Catalogs.Product> RequestsProduct { get; set; }
        //public IList<Product.Request> RequestsProduct { get; set; }
        public async  Task<IActionResult> OnGet()
        {
            TeamRequest = await _context.Teams.ToListAsync();
            ViewData["TeamMember"] = new SelectList(TeamRequest, "Name", "Name");
            //using (var client = new HttpClient())
            //{
            //    var url = _config.GetSection("baseUrl").Value;
            //    var response = await client.GetAsync(url+$"api/Common/GetAllDeveloper");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var json = await response.Content.ReadAsStringAsync();
            //        TeamRequest = JsonConvert.DeserializeObject<IList<Team.Request>>(json);

            //    }
            //    ViewData["TeamMember"] = new SelectList(TeamRequest, "Name", "Name");


            //}

            RequestsProduct = await _context.Products.ToListAsync();
            ViewData["Project"] = new SelectList(RequestsProduct, "Name", "Name");
            //using (var client = new HttpClient())
            //{
            //    var url = _config.GetSection("baseUrl").Value;
            //    var response = await client.GetAsync(url+$"api/Common/GetAllProject");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var json = await response.Content.ReadAsStringAsync();
            //        RequestsProduct = JsonConvert.DeserializeObject<IList<Product.Request>>(json);

            //    }
            //    ViewData["Project"] = new SelectList(RequestsProduct, "Name", "Name");
            //}
            return Page();
        }

        [BindProperty]
        public DailyReportRequest DailyReportRequest { get; set; }

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
                var url = _config.GetSection("baseUrl").Value;
                var request = new Model.Entity.DailyReport.DailyReport()
                {
                    Name = DailyReportRequest.Name,
                    //Email = DailyReportRequest.Email,
                    ProjectName = DailyReportRequest.ProjectName,
                    AssignDate = DailyReportRequest.AssignDate,
                    TargetDate = DailyReportRequest.TargetDate,
                    EndDate = DailyReportRequest.EndDate,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url+$"api/DailyReport/CreateDailyReport", content);
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
