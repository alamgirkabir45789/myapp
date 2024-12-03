using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.BGMEACircular
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
        public Model.Entity.NewsAndEvents.BGMEACircular BgmeaCircular { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            BgmeaCircular = await _context.BGMEACirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (BgmeaCircular == null)
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

            BgmeaCircular = await _context.BGMEACirculars.FindAsync(id);

            if (BgmeaCircular != null)
            {
                FileRemove.ImageRemove(BgmeaCircular.FilePath);
                _context.BGMEACirculars.Remove(BgmeaCircular);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
