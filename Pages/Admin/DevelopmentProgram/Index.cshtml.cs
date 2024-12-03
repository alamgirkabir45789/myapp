using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DevelopmentProgram
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Model.Entity.Career.DevelopmentProgram> DevelopmentProgram { get; set; }

        public async Task OnGetAsync()
        {
            DevelopmentProgram = await _context.DevelopmentPrograms.ToListAsync();
        }
    }
}
