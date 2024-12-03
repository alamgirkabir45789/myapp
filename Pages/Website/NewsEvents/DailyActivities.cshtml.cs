using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.NewsEvents
{
    public class DailyActivitiesModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DailyActivitiesModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.NewsAndEvents.DayToDayActivity> DayToDayActivities { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "NEWS-EVENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            DayToDayActivities = await _context.DayToDayActivities.OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
