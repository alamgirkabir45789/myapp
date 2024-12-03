using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.DevelopmentProgram
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
        public Request RequestDevelopmentProgram { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestDevelopmentProgram.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestDevelopmentProgram.File, "Uploads/Development-Program");

            Model.Entity.Career.DevelopmentProgram developmentProgram = new Model.Entity.Career.DevelopmentProgram()
            {
                Key = RequestDevelopmentProgram.Key,
                FileName = RequestDevelopmentProgram.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            _context.DevelopmentPrograms.Add(developmentProgram);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
