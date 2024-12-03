using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.NewsAndEvents;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DailyActivity
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DeleteModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayToDayActivity = await _context.DayToDayActivities.FindAsync(id);

            if (DayToDayActivity != null)
            {
                FileRemove.ImageRemove(DayToDayActivity.FilePath);
                _context.DayToDayActivities.Remove(DayToDayActivity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
