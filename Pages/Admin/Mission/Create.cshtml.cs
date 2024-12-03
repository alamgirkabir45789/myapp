using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using System.IO;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.User;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.Mission
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;
        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Request RequestMission { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            foreach (var file in RequestMission.File)
            {
                var extention = Path.GetExtension(file.FileName);
                string filePath = string.Empty;
                string message = FileSave.SaveImage(out filePath, file, "Uploads/Mission");

                Model.Entity.AboutUs.Mission mission = new Model.Entity.AboutUs.Mission()
                {
                    Key = RequestMission.Key,
                    Title = RequestMission.Title,
                    Description = RequestMission.Description,
                    FilePath = filePath,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };

                await _context.Missions.AddAsync(mission);
                var count=await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Mission has been created successfully!!!");
                    return RedirectToPage("./Index");

                }
            }
            return RedirectToPage("./Index");
        }
    }
}
