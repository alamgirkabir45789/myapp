using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Service
{
    public class ServiceModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ServiceModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Catalogs.Campaigning> Campaignings { get; set; }
        public IList<Model.Entity.Catalogs.Category> Categories { get; set; }
        public IList<Model.Entity.Catalogs.Product> Products { get; set; }
       
        public string Type { get; set; }

        public async Task OnGetAsync(string type)
        {

            Categories = await _context.Categories.ToListAsync();
            Campaignings = await _context.Campaignings.Where(x => x.BusinessType == type).ToListAsync();
            Products = await _context.Products
                                       .Include(g => g.Category)
                                        .ToListAsync();
            //Products = await _context.Products.ToListAsync();

            Type = type;
        }
    }
}
