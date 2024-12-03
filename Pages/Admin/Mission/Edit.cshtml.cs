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

namespace myportfolio.Pages.Admin.Mission
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
            _notyfService = notyfService;
        }


        [BindProperty]
        public Request RequestMission { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.AboutUs.Mission mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null)
            {
                return NotFound();
            }

            RequestMission = new Request()
            {
                Id = mission.Id,
                Key = mission.Key,
                Title = mission.Title,
                Description = mission.Description,
                FilePath = mission.FilePath,
                CreatedAt = mission.CreatedAt,
                CreatedBy = mission.CreatedBy
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
                string message = FileSave.SaveImage(out filePath, RequestMission.File[0], "Uploads/Mission");
            }



            Model.Entity.AboutUs.Mission mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == RequestMission.Id);
            mission.Title = RequestMission.Title;
            mission.Description = RequestMission.Description;
            mission.FilePath = filePath;
            mission.UpdatedAt = DateTime.UtcNow;
            mission.UpdatedBy = user.Email;

            _context.Attach(mission).State = EntityState.Modified;

            try
            {
               var count= await _context.SaveChangesAsync();
                if(count > 0)
                {
                    _notyfService.Success("Mission has been updated successfully!!!");
                    return RedirectToPage("./Index");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionExists(mission.Id))
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

        private bool MissionExists(int id)
        {
            return _context.Missions.Any(e => e.Id == id);
        }
    }
}
