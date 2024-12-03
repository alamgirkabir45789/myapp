using AspNetCoreHero.ToastNotification.Abstractions;
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

namespace myportfolio.Pages.Admin.MediaGallery
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;
        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
        }


        [BindProperty]
        public Request RequestMedia { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Resources.MediaGallery mediaGallery = await _context.MediaGallery.FirstOrDefaultAsync(m => m.Id == id);

            if (mediaGallery == null)
            {
                return NotFound();
            }

            RequestMedia = new Request()
            {
                Id = mediaGallery.Id,
                Key = mediaGallery.Key,
                FileName = mediaGallery.FileName,
                VideoLink = mediaGallery.VideoLink,
                MediaType = mediaGallery.MediaType,
                EventDate = mediaGallery.EventDate,
                FilePath = mediaGallery.FilePath,
                FileType = mediaGallery.FileType,
                CreatedAt = mediaGallery.CreatedAt,
                CreatedBy = mediaGallery.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
          
            string filePath = RequestMedia.FilePath;
            var extention = RequestMedia.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestMedia.File != null && !string.IsNullOrEmpty(RequestMedia.FilePath))
            {
                FileRemove.ImageRemove(RequestMedia.FilePath);
            }

            if (RequestMedia.File != null)
            {
                extention = Path.GetExtension(RequestMedia.File[0].FileName);
                string message = FileSave.SaveImage(out filePath, RequestMedia.File[0], "Uploads/Media-Gallery");
            }



           
            if (RequestMedia.VideoLink == null)
            {
                Model.Entity.Resources.MediaGallery mediaGallery = await _context.MediaGallery.FirstOrDefaultAsync(m => m.Id == RequestMedia.Id);
                mediaGallery.FileName = RequestMedia.FileName;
                mediaGallery.VideoLink = RequestMedia.VideoLink;
                mediaGallery.EventDate = RequestMedia.EventDate;
                mediaGallery.FileType = extention;
                mediaGallery.FilePath = filePath;
                mediaGallery.UpdatedAt = DateTime.UtcNow;
                mediaGallery.UpdatedBy = user.Email;

                _context.Attach(mediaGallery).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaGalleryExists(mediaGallery.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                Model.Entity.Resources.MediaGallery mediaGallery = await _context.MediaGallery.FirstOrDefaultAsync(m => m.Id == RequestMedia.Id);
                mediaGallery.FileName = RequestMedia.FileName;
                mediaGallery.VideoLink = RequestMedia.VideoLink;
                mediaGallery.EventDate = RequestMedia.EventDate;

                mediaGallery.FileType = extention;
                mediaGallery.FilePath = filePath;
                mediaGallery.UpdatedAt = DateTime.UtcNow;
                mediaGallery.UpdatedBy = user.Email;

                _context.Attach(mediaGallery).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

          

            return RedirectToPage("./Index");
        }

        private bool MediaGalleryExists(int id)
        {
            return _context.MediaGallery.Any(e => e.Id == id);
        }
    }
}
