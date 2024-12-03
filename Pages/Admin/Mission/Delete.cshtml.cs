using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Data;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Mission
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
        public Model.Entity.AboutUs.Mission Mission { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);

            if (Mission == null)
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

            Mission = await _context.Missions.FindAsync(id);

            if (Mission != null)
            {
                FileRemove.ImageRemove(Mission.FilePath);
                _context.Missions.Remove(Mission);
               var count= await _context.SaveChangesAsync();
                if(count > 0)
                {
                    _notyfService.Success("Mission has been deleted successfully!!!");
                    return RedirectToPage("./Index");

                }
            }

            return RedirectToPage("./Index");
        }
    }
}
