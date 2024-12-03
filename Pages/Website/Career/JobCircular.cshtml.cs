using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Career
{
    public class JobCircularModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public JobCircularModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Career.JobCircular> JobCircular { get; set; }

        public async Task OnGetAsync()
        {

            JobCircular = await _context.JobCirculars.Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow).ToListAsync();
        }
    }
}
