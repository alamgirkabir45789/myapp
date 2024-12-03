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

namespace myportfolio.Pages.Admin.Slider
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
        public Request RequestSlider { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) { return NotFound(); }

            Model.Entity.Designes.Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider == null) { return NotFound(); }

            RequestSlider = new Request()
            {
                Id = slider.Id,
                Key = slider.Key,
                Identifier = slider.Identifier,
                Heading = slider.Heading,
                Details = slider.Details,
                FilePath = slider.FilePath,
                CreatedAt = slider.CreatedAt,
                CreatedBy = slider.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            var user = await _userManager.GetUserAsync(User);
            string filePath = RequestSlider.FilePath;

            if (RequestSlider.File != null && !string.IsNullOrEmpty(RequestSlider.FilePath))
            {
                FileRemove.ImageRemove(RequestSlider.FilePath);
            }

            if (RequestSlider.File != null)
            {
                string message = FileSave.SaveLargeImage(out filePath, RequestSlider.File, "Uploads/Slider");
            }

            Model.Entity.Designes.Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == RequestSlider.Id);
            slider.Identifier = RequestSlider.Identifier;
            slider.Heading = RequestSlider.Heading;
            slider.Details = RequestSlider.Details;
            slider.FilePath = filePath;
            slider.UpdatedAt = DateTime.UtcNow;
            slider.UpdatedBy = user.Email;

            _context.Attach(slider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SliderExists(slider.Id))
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

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
