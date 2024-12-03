using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Campaigning
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
        public Model.Entity.Catalogs.Campaigning Campaigning { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Campaigning = await _context.Campaignings.FirstOrDefaultAsync(m => m.Id == id);

            if (Campaigning == null)
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

            Campaigning = await _context.Campaignings.FindAsync(id);

            if (Campaigning != null)
            {
                _context.Campaignings.Remove(Campaigning);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
