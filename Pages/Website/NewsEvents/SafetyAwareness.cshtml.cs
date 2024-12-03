using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.NewsEvents
{
    public class SafetyAwarenessModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public SafetyAwarenessModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.NewsAndEvents.SafetyAwareness> SafetyAwarenesses { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "NEWS-EVENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            SafetyAwarenesses = await _context.SafetyAwareness.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
