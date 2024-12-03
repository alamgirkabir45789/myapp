using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Career;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DroppedResume
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Resume> Resume { get; set; }

        public async Task OnGetAsync()
        {
            Resume = await _context.Resumes
                .Where(x => x.JobCircularId == null).ToListAsync();
        }
    }
}
