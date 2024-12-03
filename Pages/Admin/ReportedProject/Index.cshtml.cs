using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.DailyReport;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.ReportedProject
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public IndexModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }


        public IList<ReportedProjectRequested> ReportedProjectRequesteds { get; set; }


        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.GetAsync(url + $"api/ReportedProject/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ReportedProjectRequesteds = JsonConvert.DeserializeObject<IList<ReportedProjectRequested>>(json);

                }

            }
        }
    }
}
