using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Style
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
        public Model.Entity.Styles.Style Style { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Style = await _context.Styles
                .Include(p => p.StyleCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (Style == null)
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

            Style = await _context.Styles.FindAsync(id);

            if (Style != null)
            {
                FileRemove.ImageRemove(Style.FilePath);
                _context.Styles.Remove(Style);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
