using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Pages.Admin.EmpLeaveType;
using myportfolio.Pages.Admin.User;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.EmpLeave
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

        public IList<EmpLeaveRequest> EmpLeaveRequest { get; set; }
        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.GetAsync(url + $"api/EmpLeaveType/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    EmpLeaveRequest = JsonConvert.DeserializeObject<IList<EmpLeaveRequest>>(json);

                }

            }
        }
    }
}
