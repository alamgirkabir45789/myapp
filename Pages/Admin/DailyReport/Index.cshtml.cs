using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.DailyReport;

namespace myportfolio.Pages.Admin.DailyReport
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public IndexModel(myportfolio.Model.ApplicationDbContext context,IConfiguration config)
        {
            _context = context;
            _config = config;

        }


        public IList<DailyReportRequest> DailyReportRequests { get;set; }

      
        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.GetAsync(url+$"api/DailyReport/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    DailyReportRequests = JsonConvert.DeserializeObject< IList < DailyReportRequest >> (json);
                   
                }

            }
        }
    }
}
