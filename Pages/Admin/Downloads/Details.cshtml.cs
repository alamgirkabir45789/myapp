using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Resources;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Downloads
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
