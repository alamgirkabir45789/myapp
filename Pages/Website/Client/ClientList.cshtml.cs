using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Client
{
    public class ClientListModel : PageModel
    {


        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ClientListModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.GroupRecources.Buyer> Buyers { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }


        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "OUR-CLIENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            Buyers = await _context.Buyers.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
