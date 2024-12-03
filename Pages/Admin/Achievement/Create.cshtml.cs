using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.Achievement
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
        public Request RequestAchievement { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestAchievement.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestAchievement.File, "Uploads/Achievement");

            Model.Entity.GroupRecources.Achievement achievement = new Model.Entity.GroupRecources.Achievement()
            {
                Key = RequestAchievement.Key,
                Name = RequestAchievement.FileName,
                Achieved = RequestAchievement.Achieved,
                //FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Achievements.AddAsync(achievement);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
