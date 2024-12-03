using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.ReportedProject;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.EmpLeaveType
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public DetailsModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [BindProperty]
        public EmpLeaveTypeRequest EmpLeaveRequest { get; set; }

        public async Task OnGetAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.GetAsync(url + $"api/EmpLeaveType/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    EmpLeaveRequest = JsonConvert.DeserializeObject<EmpLeaveTypeRequest>(json);

                }

            }
        }
    }
}
