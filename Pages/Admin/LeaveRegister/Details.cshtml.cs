using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.DailyTask;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    //[Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [BindProperty]
        public LeaveRegisterRequest LeaveRegisterRequest { get; set; }


        public async Task OnGetAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.GetAsync(url+$"api/LeaveRegister/GetLeaveRegisterById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    LeaveRegisterRequest = JsonConvert.DeserializeObject<LeaveRegisterRequest>(json);

                }

            }
        }
    }
}
