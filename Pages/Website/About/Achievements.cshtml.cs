using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.About
{
    public class AchievementsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public AchievementsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.GroupRecources.Achievement> Achievements { get; set; }

        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "ABOUT-US").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            Achievements = await _context.Achievements.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
