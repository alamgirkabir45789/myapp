using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.User;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace myportfolio.Pages.Admin.History
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;
        public EditModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public Request RequestMission { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.AboutUs.History history= await _context.Histories.FirstOrDefaultAsync(m => m.Id == id);

            if (history == null)
            {
                return NotFound();
            }

            RequestMission = new Request()
            {
                Id = history.Id,
                Key = history.Key,
                Title = history.Title,
                Description = history.Description,
                FilePath = history.FilePath,
                CreatedAt = history.CreatedAt,
                CreatedBy = history.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestMission.FilePath;
            var extention = RequestMission.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestMission.File != null && !string.IsNullOrEmpty(RequestMission.FilePath))
            {
                FileRemove.ImageRemove(RequestMission.FilePath);
            }

            if (RequestMission.File != null)
            {
                extention = Path.GetExtension(RequestMission.File[0].FileName);
                string message = FileSave.SaveImage(out filePath, RequestMission.File[0], "Uploads/History");
            }



            Model.Entity.AboutUs.History history = await _context.Histories.FirstOrDefaultAsync(m => m.Id == RequestMission.Id);
            history.Title = RequestMission.Title;
            history.Description = RequestMission.Description;
            history.FilePath = filePath;
            history.UpdatedAt = DateTime.UtcNow;
            history.UpdatedBy = user.Email;

            _context.Attach(history).State = EntityState.Modified;

            try
            {
               var count= await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("History has been updated successfully!!!");
                    return RedirectToPage("./Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryExists(history.Id))
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

        private bool HistoryExists(int id)
        {
            return _context.Histories.Any(e => e.Id == id);
        }
    }
}
