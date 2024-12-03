using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;

using System;
using System.IO;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.DailyActivity
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
        public Request RequestDayToDayActivities { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestDayToDayActivities.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestDayToDayActivities.File, "Uploads/Daily-Activity");

            Model.Entity.NewsAndEvents.DayToDayActivity dayToDayActivity = new Model.Entity.NewsAndEvents.DayToDayActivity()
            {
                Key = RequestDayToDayActivities.Key,
                FileName = RequestDayToDayActivities.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.DayToDayActivities.AddAsync(dayToDayActivity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
