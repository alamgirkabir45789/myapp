using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.NewsAndEvents;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DailyActivity
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public DayToDayActivity DayToDayActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayToDayActivity = await _context.DayToDayActivities.FirstOrDefaultAsync(m => m.Id == id);

            if (DayToDayActivity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
