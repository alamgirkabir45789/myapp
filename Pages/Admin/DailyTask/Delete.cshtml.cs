using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.DailyReport;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DailyTask
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;

        private readonly INotyfService _notyfService;

        public DeleteModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config, INotyfService notyfService)
        {
            _context = context;
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

        }



        [ActionName("OnGet")]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.DeleteAsync(url+$"api/DailyTask/DeleteDailyTask?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Task has been deleted!!!");
                    return RedirectToPage("./Index");

                }

            }
            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Create", "OnGet");
        }
    }
}
