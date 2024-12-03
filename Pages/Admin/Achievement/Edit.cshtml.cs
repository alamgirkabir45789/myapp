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

namespace myportfolio.Pages.Admin.Achievement
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
        public Request RequestAchievement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.GroupRecources.Achievement achievement = await _context.Achievements.FirstOrDefaultAsync(m => m.Id == id);

            if (achievement == null)
            {
                return NotFound();
            }

            RequestAchievement = new Request()
            {
                Id = achievement.Id,
                Key = achievement.Key,
                FileName = achievement.Name,
                Achieved = achievement.Achieved,
                FilePath = achievement.FilePath,
                //FileType = achievement.FileType,
                CreatedAt = achievement.CreatedAt,
                CreatedBy = achievement.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestAchievement.FilePath;
            var extention = RequestAchievement.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestAchievement.File != null && !string.IsNullOrEmpty(RequestAchievement.FilePath))
            {
                FileRemove.ImageRemove(RequestAchievement.FilePath);
            }

            if (RequestAchievement.File != null)
            {
                extention = Path.GetExtension(RequestAchievement.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestAchievement.File, "Uploads/Achievement");
            }



            Model.Entity.GroupRecources.Achievement achievement = await _context.Achievements.FirstOrDefaultAsync(m => m.Id == RequestAchievement.Id);
            achievement.Name = RequestAchievement.FileName;
            achievement.Achieved = RequestAchievement.Achieved;
            //buyer.FileType = extention;
            achievement.FilePath = filePath;
            achievement.UpdatedAt = DateTime.UtcNow;
            achievement.UpdatedBy = user.Email;

            _context.Attach(achievement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementExists(achievement.Id))
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
        private bool AchievementExists(int id)
        {
            return _context.Achievements.Any(e => e.Id == id);
        }
    }
}
