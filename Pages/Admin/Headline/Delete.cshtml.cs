using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.Designes;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Headline
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
        public HeadLine HeadLine { get; set; }

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    HeadLine = await _context.HeadLines.FirstOrDefaultAsync(m => m.Id == id);

        //    if (HeadLine == null)
        //    {
        //        return NotFound();
        //    }
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HeadLine = await _context.HeadLines.FindAsync(id);

            if (HeadLine != null)
            {
                _context.HeadLines.Remove(HeadLine);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
