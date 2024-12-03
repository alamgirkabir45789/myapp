using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Slider
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
        public Request RequestSlider { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            var user = await _userManager.GetUserAsync(User);
            string filePath = string.Empty;
            string message = FileSave.SaveLargeImage(out filePath, RequestSlider.File, "Uploads/Slider");

            Model.Entity.Designes.Slider slider = new Model.Entity.Designes.Slider()
            {
                Key = RequestSlider.Key,
                Identifier = RequestSlider.Identifier,
                Heading = RequestSlider.Heading,
                Details = RequestSlider.Details,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
