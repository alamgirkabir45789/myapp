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

namespace myportfolio.Pages.Admin.DevelopmentProgram
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
        public Request RequestDevelopmentProgram { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Career.DevelopmentProgram developmentProgram = await _context.DevelopmentPrograms.FirstOrDefaultAsync(m => m.Id == id);

            if (developmentProgram == null)
            {
                return NotFound();
            }

            RequestDevelopmentProgram = new Request()
            {
                Id = developmentProgram.Id,
                Key = developmentProgram.Key,
                FileName = developmentProgram.FileName,
                FilePath = developmentProgram.FilePath,
                FileType = developmentProgram.FileType,
                CreatedAt = developmentProgram.CreatedAt,
                CreatedBy = developmentProgram.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestDevelopmentProgram.FilePath;
            var extention = RequestDevelopmentProgram.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestDevelopmentProgram.File != null && !string.IsNullOrEmpty(RequestDevelopmentProgram.FilePath))
            {
                FileRemove.ImageRemove(RequestDevelopmentProgram.FilePath);
            }

            if (RequestDevelopmentProgram.File != null)
            {
                extention = Path.GetExtension(RequestDevelopmentProgram.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestDevelopmentProgram.File, "Uploads/Development-Program");
            }



            Model.Entity.Career.DevelopmentProgram developmentProgram = await _context.DevelopmentPrograms.FirstOrDefaultAsync(m => m.Id == RequestDevelopmentProgram.Id);
            developmentProgram.FileName = RequestDevelopmentProgram.FileName;
            developmentProgram.FileType = extention;
            developmentProgram.FilePath = filePath;
            developmentProgram.UpdatedAt = DateTime.UtcNow;
            developmentProgram.UpdatedBy = user.Email;

            _context.Attach(developmentProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevelopmentProgramExists(developmentProgram.Id))
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

        private bool DevelopmentProgramExists(int id)
        {
            return _context.DevelopmentPrograms.Any(e => e.Id == id);
        }
    }
}
