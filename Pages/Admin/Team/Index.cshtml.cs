using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Team
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Team.Team> Teams { get; set; }

        public async Task OnGetAsync()
        {
            //Teams = await _context.Teams.OrderByDescending(x=>x.Name).ToListAsync();
            Teams = await _context.Teams.OrderBy(x => x.SortCode).ToListAsync();
        }
    }
}
