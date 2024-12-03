using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.GroupRecources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Company
{
    [Authorize]
    public class CompanyImageModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public CompanyImageModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CompanyImage> CompanyImages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ViewData["CompanyId"] = id;
            CompanyImages = await _context.CompanyImages.Where(x => x.CompanyId == id).ToListAsync();
            return Page();
        }
    }
}
