using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.About
{
    public class ComplianceModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ComplianceModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.AboutUs.Compliance> Compliances { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "ABOUT-US").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            Compliances = await _context.Compliances.ToListAsync();
        }
    }
}
