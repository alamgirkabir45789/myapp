using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.SafetyAwareness
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
        public Model.Entity.NewsAndEvents.SafetyAwareness SafetyAwareness { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SafetyAwareness = await _context.SafetyAwareness.FirstOrDefaultAsync(m => m.Id == id);

            if (SafetyAwareness == null)
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

            SafetyAwareness = await _context.SafetyAwareness.FindAsync(id);

            if (SafetyAwareness != null)
            {
                FileRemove.ImageRemove(SafetyAwareness.FilePath);
                _context.SafetyAwareness.Remove(SafetyAwareness);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
