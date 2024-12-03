using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.EmpLeaveType
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyfService;
        public DeleteModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config,INotyfService notyfService)
        {
            _context = context;
            _config = config;
            _notyfService = notyfService;
        }

        [BindProperty]
        public EmpLeaveTypeRequest EmpLeaveRequest { get; set; }

        public async Task OnGet(int? id)
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



        [ActionName("OnGet")]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;

                var response = await client.DeleteAsync(url + $"api/EmpLeaveType/DeleteEmpLeaveType?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    _notyfService.Success("Leave type has been deleted successfully!!!");
                    return RedirectToPage("./Index");

                }

            }
            return Page();

        }
    }
}
