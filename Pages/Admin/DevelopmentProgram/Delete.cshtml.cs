using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DevelopmentProgram
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
        public Model.Entity.Career.DevelopmentProgram DevelopmentProgram { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DevelopmentProgram = await _context.DevelopmentPrograms.FirstOrDefaultAsync(m => m.Id == id);

            if (DevelopmentProgram == null)
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

            DevelopmentProgram = await _context.DevelopmentPrograms.FindAsync(id);

            if (DevelopmentProgram != null)
            {
                FileRemove.ImageRemove(DevelopmentProgram.FilePath);
                _context.DevelopmentPrograms.Remove(DevelopmentProgram);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
