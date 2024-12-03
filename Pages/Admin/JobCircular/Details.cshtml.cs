using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace myportfolio.Pages.Admin.JobCircular
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.Career.JobCircular JobCircular { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobCircular = await _context.JobCirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (JobCircular == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
