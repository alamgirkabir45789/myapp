using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Product
{
    public class ProductModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ProductModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Catalogs.Category> Categories { get; set; }
        public IList<Model.Entity.Catalogs.Product> Products { get; set; }

        public string Type { get; set; }

        public async Task OnGetAsync(string type)
        {
            
            Categories = await _context.Categories.ToListAsync();
           
            Products = await _context.Products
                                       .Include(g => g.Category)
                                        .ToListAsync();
            //Products = await _context.Products.ToListAsync();

            Type = type;
        }
    }
}
