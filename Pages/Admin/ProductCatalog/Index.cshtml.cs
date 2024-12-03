using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.ProductCatalog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Resources.ProductCatalog> ProductCatalog { get; set; }

        public async Task OnGetAsync()
        {
            ProductCatalog = await _context.ProductCatalogs
                .Include(d => d.FileGroup).ToListAsync();
        }
    }
}
