using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.Compliance
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
        public Request RequestCompliance { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestCompliance.File.FileName);
            string filePath = string.Empty;
            string imagePath = string.Empty;
            string message = FileSave.SaveDocument(out filePath, RequestCompliance.File, "Uploads/Compliance");
            string messageImage = FileSave.SaveImage(out imagePath, RequestCompliance.Image, "Uploads/Compliance");

            Model.Entity.AboutUs.Compliance compliance = new Model.Entity.AboutUs.Compliance()
            {
                Key = RequestCompliance.Key,
                Title = RequestCompliance.Title,
                Details = RequestCompliance.Details,
                //FileType = extention,
                FilePath = filePath,
                ImagePath = imagePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Compliances.AddAsync(compliance);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
