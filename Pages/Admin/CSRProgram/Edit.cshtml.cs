using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.CSRProgram
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
        public Request RequestCSRProgram { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.NewsAndEvents.CSRProgram csrProgram = await _context.CSRPrograms.FirstOrDefaultAsync(m => m.Id == id);

            if (csrProgram == null)
            {
                return NotFound();
            }

            RequestCSRProgram = new Request()
            {
                Id = csrProgram.Id,
                Key = csrProgram.Key,
                FileName = csrProgram.FileName,
                FilePath = csrProgram.FilePath,
                FileType = csrProgram.FileType,
                CreatedAt = csrProgram.CreatedAt,
                CreatedBy = csrProgram.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestCSRProgram.FilePath;
            var extention = RequestCSRProgram.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestCSRProgram.File != null && !string.IsNullOrEmpty(RequestCSRProgram.FilePath))
            {
                FileRemove.ImageRemove(RequestCSRProgram.FilePath);
            }

            if (RequestCSRProgram.File != null)
            {
                extention = Path.GetExtension(RequestCSRProgram.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestCSRProgram.File, "Uploads/CSR-Program");
            }



            Model.Entity.NewsAndEvents.CSRProgram csrProgram = await _context.CSRPrograms.FirstOrDefaultAsync(m => m.Id == RequestCSRProgram.Id);
            csrProgram.FileName = RequestCSRProgram.FileName;
            csrProgram.FileType = extention;
            csrProgram.FilePath = filePath;
            csrProgram.UpdatedAt = DateTime.UtcNow;
            csrProgram.UpdatedBy = user.Email;

            _context.Attach(csrProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CSRProgramExists(csrProgram.Id))
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




        private bool CSRProgramExists(int id)
        {
            return _context.CSRPrograms.Any(e => e.Id == id);
        }
    }
}
