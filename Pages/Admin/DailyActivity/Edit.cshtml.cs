using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.DailyActivity
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
        public Request RequestDayToDayActivities { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.NewsAndEvents.DayToDayActivity dayToDayActivity = await _context.DayToDayActivities.FirstOrDefaultAsync(m => m.Id == id);

            if (dayToDayActivity == null)
            {
                return NotFound();
            }

            RequestDayToDayActivities = new Request()
            {
                Id = dayToDayActivity.Id,
                Key = dayToDayActivity.Key,
                FileName = dayToDayActivity.FileName,
                FilePath = dayToDayActivity.FilePath,
                FileType = dayToDayActivity.FileType,
                CreatedAt = dayToDayActivity.CreatedAt,
                CreatedBy = dayToDayActivity.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestDayToDayActivities.FilePath;
            var extention = RequestDayToDayActivities.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestDayToDayActivities.File != null && !string.IsNullOrEmpty(RequestDayToDayActivities.FilePath))
            {
                FileRemove.ImageRemove(RequestDayToDayActivities.FilePath);
            }

            if (RequestDayToDayActivities.File != null)
            {
                extention = Path.GetExtension(RequestDayToDayActivities.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestDayToDayActivities.File, "Uploads/Daily-Activity");
            }



            Model.Entity.NewsAndEvents.DayToDayActivity dayToDayActivity = await _context.DayToDayActivities.FirstOrDefaultAsync(m => m.Id == RequestDayToDayActivities.Id);
            dayToDayActivity.FileName = RequestDayToDayActivities.FileName;
            dayToDayActivity.FileType = extention;
            dayToDayActivity.FilePath = filePath;
            dayToDayActivity.UpdatedAt = DateTime.UtcNow;
            dayToDayActivity.UpdatedBy = user.Email;

            _context.Attach(dayToDayActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayToDayActivityExists(dayToDayActivity.Id))
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

        private bool DayToDayActivityExists(int id)
        {
            return _context.DayToDayActivities.Any(e => e.Id == id);
        }
    }
}
