using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.MediaGallery
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;

        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Request RequestMedia { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if(RequestMedia.File != null)
            {
                foreach (var file in RequestMedia.File)
                {
                    var extention = Path.GetExtension(file.FileName);
                    string filePath = string.Empty;
                    string message = FileSave.SaveImage(out filePath, file, "Uploads/Media-Gallery");

                    Model.Entity.Resources.MediaGallery mediaGallery = new Model.Entity.Resources.MediaGallery()
                    {
                        Key = RequestMedia.Key,
                        FileName = RequestMedia.FileName,
                        MediaType = RequestMedia.MediaType,
                        EventDate = RequestMedia.EventDate,
                        VideoLink = RequestMedia.VideoLink,
                        FileType = extention,
                        FilePath = filePath,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Email
                    };

                    await _context.MediaGallery.AddAsync(mediaGallery);
                   var count= await _context.SaveChangesAsync();
                    if (count > 0)
                    {
                        _notyfService.Success("Gallery has been submitted successfully!!!");
                        return RedirectToPage("./Index");
                    }
                }
            }
            else
            {
               
                    

                    Model.Entity.Resources.MediaGallery mediaGallery = new Model.Entity.Resources.MediaGallery()
                    {
                        Key = RequestMedia.Key,
                        FileName = RequestMedia.FileName,
                        MediaType = RequestMedia.MediaType,
                        EventDate = RequestMedia.EventDate,
                        VideoLink = RequestMedia.VideoLink,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Email
                    };

                    await _context.MediaGallery.AddAsync(mediaGallery);
                   var count= await _context.SaveChangesAsync();
                if(count > 0)
                {
                    _notyfService.Success("Gallery has been submitted successfully!!!");
                    return RedirectToPage("./Index");
                }
                
            }

           
            return RedirectToPage("./Index");
        }
    }
}
