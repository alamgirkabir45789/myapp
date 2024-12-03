using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Compliance
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
