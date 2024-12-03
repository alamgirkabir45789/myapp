using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Buyer
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.GroupRecources.Buyer Buyer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Buyer = await _context.Buyers.FirstOrDefaultAsync(m => m.Id == id);

            if (Buyer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
