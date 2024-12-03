using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.SafetyAwareness
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.NewsAndEvents.SafetyAwareness> SafetyAwareness { get; set; }

        public async Task OnGetAsync()
        {
            SafetyAwareness = await _context.SafetyAwareness.ToListAsync();
        }
    }
}
