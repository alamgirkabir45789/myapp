using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.MediaGallery
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.Resources.MediaGallery MediaGallery { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MediaGallery = await _context.MediaGallery.FirstOrDefaultAsync(m => m.Id == id);

            if (MediaGallery == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
