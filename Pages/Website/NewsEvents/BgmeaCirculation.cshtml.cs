using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.NewsEvents
{
    public class BgmeaCirculationModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public BgmeaCirculationModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.NewsAndEvents.BGMEACircular> BgmeaCirculars { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "NEWS-EVENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            BgmeaCirculars = await _context.BGMEACirculars.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
