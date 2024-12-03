﻿using System;
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

namespace myportfolio.Pages.Admin.DailyReport
{
    [Authorize(Roles = "Admin")]
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
        public DailyReportRequest DailyReportRequest { get; set; }

        public async Task OnGetAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var url = _config.GetSection("baseUrl").Value;
                var response = await client.GetAsync(url + $"api/DailyReport/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    DailyReportRequest = JsonConvert.DeserializeObject<DailyReportRequest>(json);

                }

            }
        }
    }
}