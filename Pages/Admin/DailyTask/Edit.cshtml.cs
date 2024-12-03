using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.DailyReport;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.User;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Linq;

namespace myportfolio.Pages.Admin.DailyTask
{
   [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;

        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _notyfService = notyfService;
        }

        [BindProperty]
        public DailyTaskReques DailyTaskReques { get; set; }

        public async Task OnGet(int? id)
        {

            var url = _config.GetSection("baseUrl").Value;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url+$"api/DailyTask/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    DailyTaskReques = JsonConvert.DeserializeObject<DailyTaskReques>(json);
              
                }

            }
            var user = await _userManager.GetUserAsync(User);
            ViewData["user"] = user.Name;
            var DailyReportRequests = await _context.ReportedProjects.ToListAsync();
           
            ViewData["ReportedProject"] = new SelectList(DailyReportRequests, "ProjectName", "ProjectName");

        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                //var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == DailyTaskReques.UserId);
                //var userName = await _userManager.FindByIdAsync(selectedUser.Name);

                string project = DailyTaskReques.WorkingProject;
                string[] projects = project.Split(',');
                //string[] myInts = projects;
                var workingProject = "";
                for (int i = 0; i < projects.Length; i++)
                {
                    string pjName = (projects[i]);
                    if (pjName != null)
                    {
                        var prName = _context.ReportedProjects.FirstOrDefault(x => x.ProjectName == pjName);
                        if (prName != null)
                        {
                            workingProject += prName.Id + ",";
                        }
                    }
                }

                var modifiedWorkingProjectId = workingProject.Remove(workingProject.Length - 1);
             

                DailyTaskReques.WorkingProject = modifiedWorkingProjectId;


                var request = new DailyTaskReques()
                {
                    Name = DailyTaskReques.Name,
                    UserId = DailyTaskReques.UserId,
                    Description = DailyTaskReques.Description,
                    SubmitDate = DailyTaskReques.SubmitDate,
                    InTime= DailyTaskReques.InTime,
                    OutTime=DailyTaskReques.OutTime,
                    WorkingProject = DailyTaskReques.WorkingProject,   
                    IsHoliday=DailyTaskReques.IsHoliday,
                    Key = DailyTaskReques.Key,
                    Id = DailyTaskReques.Id,
                    CreatedAt = DailyTaskReques.CreatedAt,
                    CreatedBy = DailyTaskReques.CreatedBy,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = user.Email
                };
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url+$"api/DailyTask/UpdateDailyTask", content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Task has been updated successfully!!!");
                    return RedirectToPage("./Index");
                }

            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Create", "OnGet");
        }

    }
}
