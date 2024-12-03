using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Downloads
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
        public Request RequestDownloads { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Resources.Download download = await _context.Downloads.FirstOrDefaultAsync(m => m.Id == id);

            if (download == null)
            {
                return NotFound();
            }

            ViewData["FileGroupId"] = new SelectList(_context.FileGroups.Where(x => x.GroupFor == "download"), "Id", "GroupName");

            RequestDownloads = new Request()
            {
                Id = download.Id,
                Key = download.Key,
                FileName = download.FileName,
                FilePath = download.FilePath,
                FileType = download.FileType,
                CreatedAt = download.CreatedAt,
                CreatedBy = download.CreatedBy,
                FileGroupId = download.FileGroupId
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestDownloads.FilePath;
            var extention = RequestDownloads.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestDownloads.File != null && !string.IsNullOrEmpty(RequestDownloads.FilePath))
            {
                FileRemove.ImageRemove(RequestDownloads.FilePath);
            }

            if (RequestDownloads.File != null)
            {
                extention = Path.GetExtension(RequestDownloads.File.FileName);
                string message = FileSave.SaveDocument(out filePath, RequestDownloads.File, "Uploads/Downloads");
            }



            Model.Entity.Resources.Download download = await _context.Downloads.FirstOrDefaultAsync(m => m.Id == RequestDownloads.Id);
            download.FileName = RequestDownloads.FileName;
            download.FileType = extention;
            download.FilePath = filePath;
            download.UpdatedAt = DateTime.UtcNow;
            download.UpdatedBy = user.Email;

            _context.Attach(download).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DownloadExists(download.Id))
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




        private bool DownloadExists(int id)
        {
            return _context.Downloads.Any(e => e.Id == id);
        }
    }
}
