using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    [Authorize(Roles = "Admin,Employee,Developer")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public IndexModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
           
        }
   

        public IList<LeaveRegisterRequest> LeaveRegisterRequests { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = User.IsInRole("Admin");
            var url = _config.GetSection("baseUrl").Value;
            var userList=await _context.Users.ToListAsync();
            
            ViewData["userList"] = new SelectList(userList, "Id", "Name");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url+ $"api/LeaveRegister/GetLeaveRegisters");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resData = JsonConvert.DeserializeObject<IList<LeaveRegisterRequest>>(json);
                    if (isAdmin)
                    {
                        LeaveRegisterRequests = resData;
                    }
                    else
                    {
                        LeaveRegisterRequests=resData.Where(x=>x.UserId==user.Id).ToList();
                    }
                }
            }         
            return Page();
        }
      
    }
}
