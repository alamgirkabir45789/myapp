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

namespace myportfolio.Pages.Admin.BGMEACircular
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
        public Request RequestBgmeaCircular { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.NewsAndEvents.BGMEACircular bgmeaCircular = await _context.BGMEACirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (bgmeaCircular == null)
            {
                return NotFound();
            }


            RequestBgmeaCircular = new Request()
            {
                Id = bgmeaCircular.Id,
                Key = bgmeaCircular.Key,
                FileName = bgmeaCircular.CircularTitle,
                CreatedAt = bgmeaCircular.CreatedAt,
                CreatedBy = bgmeaCircular.CreatedBy,
                FileType = bgmeaCircular.FileType,
                FilePath = bgmeaCircular.FilePath,
                Language = bgmeaCircular.Language,
                CircularDetails = bgmeaCircular.CircularDetails,
                PublishDate = bgmeaCircular.PublishDate

            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestBgmeaCircular.FilePath;
            var extention = RequestBgmeaCircular.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestBgmeaCircular.File != null && !string.IsNullOrEmpty(RequestBgmeaCircular.FilePath))
            {
                FileRemove.ImageRemove(RequestBgmeaCircular.FilePath);
            }

            if (RequestBgmeaCircular.File != null)
            {
                extention = Path.GetExtension(RequestBgmeaCircular.File.FileName);
                string message = FileSave.SaveDocument(out filePath, RequestBgmeaCircular.File, "Uploads/BGMEA-Circular");
            }



            Model.Entity.NewsAndEvents.BGMEACircular bgmeaCircular = await _context.BGMEACirculars.FirstOrDefaultAsync(m => m.Id == RequestBgmeaCircular.Id);
            bgmeaCircular.CircularTitle = RequestBgmeaCircular.FileName;
            bgmeaCircular.FileType = extention;
            bgmeaCircular.FilePath = filePath;
            bgmeaCircular.UpdatedAt = DateTime.UtcNow;
            bgmeaCircular.UpdatedBy = user.Email;
            bgmeaCircular.PublishDate = RequestBgmeaCircular.PublishDate;
            bgmeaCircular.CircularDetails = RequestBgmeaCircular.CircularDetails;
            bgmeaCircular.Language = RequestBgmeaCircular.Language;


            _context.Attach(bgmeaCircular).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BgmeaCircularExists(bgmeaCircular.Id))
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




        private bool BgmeaCircularExists(int id)
        {
            return _context.BGMEACirculars.Any(e => e.Id == id);
        }
    }
}
