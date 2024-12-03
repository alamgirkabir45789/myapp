using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Category
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly INotyfService _notifyService;
        public DeleteModel(myportfolio.Model.ApplicationDbContext context,INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }

        [BindProperty]
        public Model.Entity.Catalogs.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
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

            Category = await _context.Categories.FindAsync(id);

            if (Category != null)
            {
                _context.Categories.Remove(Category);
              var count=  await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notifyService.Success("Data has been deleted successfully!!!");
                    return RedirectToPage("./Index");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
