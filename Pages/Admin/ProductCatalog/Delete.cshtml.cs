using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.ProductCatalog
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
        public Model.Entity.Resources.ProductCatalog ProductCatalog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ProductCatalog = await _context.ProductCatalogs
                .Include(d => d.FileGroup).FirstOrDefaultAsync(m => m.Id == id);

            if (ProductCatalog == null)
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

            ProductCatalog = await _context.ProductCatalogs.FindAsync(id);

            if (ProductCatalog != null)
            {
                FileRemove.ImageRemove(ProductCatalog.FilePath);
                _context.ProductCatalogs.Remove(ProductCatalog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
