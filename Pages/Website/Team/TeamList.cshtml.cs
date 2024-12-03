using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Team
{
  
    public class TeamListModel : PageModel
    {


        private readonly myportfolio.Model.ApplicationDbContext _context;

        public TeamListModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Team.Team> Teams { get; set; }
        //public Model.Entity.Designes.Slider Slider { get; set; }


        public async Task OnGetAsync()
        {
            //Slider = _context.Sliders.Where(x => x.Identifier == "OUR-CLIENTS").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            Teams = await _context.Teams.ToListAsync();
        }
    }
}
