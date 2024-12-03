using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.NewsAndEvents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DailyActivity
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DayToDayActivity> DayToDayActivity { get; set; }

        public async Task OnGetAsync()
        {
            DayToDayActivity = await _context.DayToDayActivities.ToListAsync();
        }
    }
}
