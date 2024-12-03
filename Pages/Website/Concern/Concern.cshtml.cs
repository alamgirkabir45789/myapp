using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.GroupRecources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Concern
{
    public class ConcernModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ConcernModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }


        public Model.Entity.GroupRecources.Company Company { get; set; }
        public IList<CompanyImage> CompanyImages { get; set; }
        public IList<Model.Entity.GroupRecources.Company> Companies { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }
        public int CompanyId { get; set; }

        public async Task OnGetAsync(int id)
        {
            CompanyId = id;
            Slider = _context.Sliders.Where(x => x.Identifier == "OUR-CONCERN").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            Companies = await _context.Companies.ToListAsync();


            Company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            CompanyImages = await _context.CompanyImages.ToListAsync();
        }

    }
}
