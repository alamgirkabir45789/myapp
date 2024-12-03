using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace myportfolio.Pages.Admin.JobCircular
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly INotyfService _notyfService;

        public DeleteModel(myportfolio.Model.ApplicationDbContext context,INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [BindProperty]
        public Model.Entity.Career.JobCircular JobCircular { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobCircular = await _context.JobCirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (JobCircular == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobCircular = await _context.JobCirculars.FindAsync(id);

            if (JobCircular != null)
            {
                _context.JobCirculars.Remove(JobCircular);
              var count=  await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Circular has been remove successfully!!!");
                    return RedirectToPage("./Index");

                }
            }

            return RedirectToPage("./Index");
        }
    }
}
