using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;

using System;
using System.IO;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.BGMEACircular
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public IActionResult OnGet()
        {
            return Page();
        }


        [BindProperty]
        public Request RequestBgmeaCircular { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var user = await _userManager.GetUserAsync(User);
            var extention = "";
            string filePath = string.Empty;

            if (RequestBgmeaCircular.File != null)
            {
                extention = Path.GetExtension(RequestBgmeaCircular.File.FileName);
                string message = FileSave.SaveDocument(out filePath, RequestBgmeaCircular.File, "Uploads/BGMEA-Circular");
            }



            Model.Entity.NewsAndEvents.BGMEACircular bgmeaCircular = new Model.Entity.NewsAndEvents.BGMEACircular()
            {

                Key = RequestBgmeaCircular.Key,
                CircularTitle = RequestBgmeaCircular.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email,
                Language = RequestBgmeaCircular.Language,
                CircularDetails = RequestBgmeaCircular.CircularDetails,
                PublishDate = RequestBgmeaCircular.PublishDate
            };

            await _context.BGMEACirculars.AddAsync(bgmeaCircular);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
