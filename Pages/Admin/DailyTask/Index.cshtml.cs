using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyReport;

namespace myportfolio.Pages.Admin.DailyTask
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public IndexModel(myportfolio.Model.ApplicationDbContext context,UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;

        }


        public IList<DailyTaskReques> DailyTaskReques { get; set; }


        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["user"] = user.Name;
            bool isAdmin = User.IsInRole("Admin");
            //var data=await _context.DailyTasks.Include(x=>x.DailyReport).ToListAsync();
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                client.Timeout = TimeSpan.FromMinutes(3);

                var response = await client.GetAsync(url+$"api/DailyTask/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resData = JsonConvert.DeserializeObject<IList<DailyTaskReques>>(json);
                    if (isAdmin)
                    {
                        DailyTaskReques = resData.OrderByDescending(x => x.SubmitDate).ToList();
                    }
                    else
                    {
                        DailyTaskReques = resData.Where(x => x.UserId == user.Id).OrderByDescending(x => x.SubmitDate).ToList();
                    }

                }

            }
        }
    }
}
