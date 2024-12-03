using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.MediaGallery
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
        public Model.Entity.Resources.MediaGallery MediaGallery { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MediaGallery = await _context.MediaGallery.FirstOrDefaultAsync(m => m.Id == id);

            if (MediaGallery == null)
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

            MediaGallery = await _context.MediaGallery.FindAsync(id);

            if (MediaGallery.FilePath != null)
            {
                FileRemove.ImageRemove(MediaGallery.FilePath);
                _context.MediaGallery.Remove(MediaGallery);
               var count= await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Gallery has been deleted successfully!!!");
                    return RedirectToPage("./Index");
                }
            }
            else
            {
                _context.MediaGallery.Remove(MediaGallery);
                var count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Gallery has been deleted successfully!!!");
                    return RedirectToPage("./Index");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
