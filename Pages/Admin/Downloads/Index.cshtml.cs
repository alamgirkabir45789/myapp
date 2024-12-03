using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Downloads
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Download> Download { get; set; }

        public async Task OnGetAsync()
        {
            Download = await _context.Downloads
                .Include(d => d.FileGroup).ToListAsync();
        }
    }
}
