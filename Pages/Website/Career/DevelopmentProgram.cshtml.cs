using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Career
{
    public class DevelopmentProgramModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DevelopmentProgramModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Career.DevelopmentProgram> DevelopmentPrograms { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "CAREER").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            DevelopmentPrograms = await _context.DevelopmentPrograms.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
