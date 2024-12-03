using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.NewsEvents
{
    public class BgmeaCircularDetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public BgmeaCircularDetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.NewsAndEvents.BGMEACircular BGMEACircular { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "NEWS-EVENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            if (id == null)
            {
                return NotFound();
            }

            BGMEACircular = await _context.BGMEACirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (BGMEACircular == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
