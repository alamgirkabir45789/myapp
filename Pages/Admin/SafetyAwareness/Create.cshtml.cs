using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.SafetyAwareness
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
        public Request RequestSafetyAwareness { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestSafetyAwareness.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestSafetyAwareness.File, "Uploads/Safety-Awareness");

            Model.Entity.NewsAndEvents.SafetyAwareness safetyAwareness = new Model.Entity.NewsAndEvents.SafetyAwareness()
            {
                Key = RequestSafetyAwareness.Key,
                FileName = RequestSafetyAwareness.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.SafetyAwareness.AddAsync(safetyAwareness);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
