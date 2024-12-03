using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Career;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.JobCircular
{
    [Authorize(Roles = "Admin")]
    public class ApplicantListModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ApplicantListModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Resume> Resumes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //ViewData["JobCircularId"] = id;
            Resumes = await _context.Resumes.Where(x => x.JobCircularId == id).ToListAsync();
            return Page();
        }
    }
}
