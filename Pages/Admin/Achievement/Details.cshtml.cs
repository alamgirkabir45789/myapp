using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Achievement
{
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.GroupRecources.Achievement Achievement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Achievement = await _context.Achievements.FirstOrDefaultAsync(m => m.Id == id);

            if (Achievement == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
