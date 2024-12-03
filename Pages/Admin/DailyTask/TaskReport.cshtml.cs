using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DailyTask
{
    [Authorize(Roles = "Admin")]
    public class TaskReportModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public TaskReportModel(myportfolio.Model.ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }
        public IList<DailyTaskReques> DailyTaskReques { get; set; }
        public IList<TaskReportViewModel> TaskReports { get; set; }

        //public IActionResult OnGet(string name,)
        //{
        //    var formatedDate = date.ToString("yyyy-MM-dd");
        //    return Page();
        //}

        public async Task OnGet(DateTime date,string id)
         {
            string input = id;
            string cleanedInput = input.Remove(startIndex: 0, count: 5);
            await Console.Out.WriteLineAsync(cleanedInput);
            var date2 = Convert.ToDateTime(cleanedInput);
            var formatedDate = date.ToString("yyyy-MM-dd");
            var formatedDate2 = date2.ToString("yyyy-MM-dd");
            ViewData["SelectedDate"] = cleanedInput;
            //var data=await _context.DailyTasks.Include(x=>x.DailyReport).ToListAsync();
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var response = await client.GetAsync(url + $"api/DailyTask/TaskReportByDate?date={formatedDate2}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resData = JsonConvert.DeserializeObject<IList<TaskReportViewModel>>(json);
                    //DailyTaskReques = JsonConvert.DeserializeObject<IList<DailyTaskReques>>(json);
                    TaskReports=resData.OrderBy(x=>x.SortCode).ToList();
                }

            }
            ViewData["selectedDate"] = date2;
        }

    }
}
