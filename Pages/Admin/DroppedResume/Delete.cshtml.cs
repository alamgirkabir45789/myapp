using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Career;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DroppedResume
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
        public Resume Resume { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Resume = await _context.Resumes
                .Include(r => r.JobCircular).FirstOrDefaultAsync(m => m.Id == id);

            if (Resume == null)
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

            Resume = await _context.Resumes.FindAsync(id);

            if (Resume != null)
            {
                FileRemove.ImageRemove(Resume.FilePath);
                _context.Resumes.Remove(Resume);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
