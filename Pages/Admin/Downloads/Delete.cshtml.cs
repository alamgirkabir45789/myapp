using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Resources;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Downloads
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
        public Download Download { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Download = await _context.Downloads
                .Include(d => d.FileGroup).FirstOrDefaultAsync(m => m.Id == id);

            if (Download == null)
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

            Download = await _context.Downloads.FindAsync(id);

            if (Download != null)
            {
                FileRemove.ImageRemove(Download.FilePath);
                _context.Downloads.Remove(Download);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
