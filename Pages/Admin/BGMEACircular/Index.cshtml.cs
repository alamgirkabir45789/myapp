using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.BGMEACircular
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.NewsAndEvents.BGMEACircular> BgmeaCircular { get; set; }

        public async Task OnGetAsync()
        {
            BgmeaCircular = await _context.BGMEACirculars
                .ToListAsync();
        }
    }
}
