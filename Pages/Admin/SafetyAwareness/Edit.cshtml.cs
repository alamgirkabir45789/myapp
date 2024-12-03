using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.SafetyAwareness
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public Request RequestSafetyAwareness { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.NewsAndEvents.SafetyAwareness safetyAwareness = await _context.SafetyAwareness.FirstOrDefaultAsync(m => m.Id == id);

            if (safetyAwareness == null)
            {
                return NotFound();
            }

            RequestSafetyAwareness = new Request()
            {
                Id = safetyAwareness.Id,
                Key = safetyAwareness.Key,
                FileName = safetyAwareness.FileName,
                FilePath = safetyAwareness.FilePath,
                FileType = safetyAwareness.FileType,
                CreatedAt = safetyAwareness.CreatedAt,
                CreatedBy = safetyAwareness.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestSafetyAwareness.FilePath;
            var extention = RequestSafetyAwareness.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestSafetyAwareness.File != null && !string.IsNullOrEmpty(RequestSafetyAwareness.FilePath))
            {
                FileRemove.ImageRemove(RequestSafetyAwareness.FilePath);
            }

            if (RequestSafetyAwareness.File != null)
            {
                extention = Path.GetExtension(RequestSafetyAwareness.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestSafetyAwareness.File, "Uploads/Safety-Awareness");
            }



            Model.Entity.NewsAndEvents.SafetyAwareness safetyAwareness = await _context.SafetyAwareness.FirstOrDefaultAsync(m => m.Id == RequestSafetyAwareness.Id);
            safetyAwareness.FileName = RequestSafetyAwareness.FileName;
            safetyAwareness.FileType = extention;
            safetyAwareness.FilePath = filePath;
            safetyAwareness.UpdatedAt = DateTime.UtcNow;
            safetyAwareness.UpdatedBy = user.Email;

            _context.Attach(safetyAwareness).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SafetyAwarenessExists(safetyAwareness.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SafetyAwarenessExists(int id)
        {
            return _context.SafetyAwareness.Any(e => e.Id == id);
        }
    }
}
