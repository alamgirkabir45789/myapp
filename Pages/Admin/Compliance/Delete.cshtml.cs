using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Compliance
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
        public Model.Entity.AboutUs.Compliance Compliance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Compliance = await _context.Compliances.FirstOrDefaultAsync(m => m.Id == id);

            if (Compliance == null)
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

            Compliance = await _context.Compliances.FindAsync(id);

            if (Compliance != null)
            {
                FileRemove.ImageRemove(Compliance.FilePath);
                FileRemove.ImageRemove(Compliance.ImagePath);
                _context.Compliances.Remove(Compliance);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
