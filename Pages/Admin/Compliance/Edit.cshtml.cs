using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Compliance
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
        public Request RequestCompliance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.AboutUs.Compliance compliance = await _context.Compliances.FirstOrDefaultAsync(m => m.Id == id);

            if (compliance == null)
            {
                return NotFound();
            }

            RequestCompliance = new Request()
            {
                Id = compliance.Id,
                Key = compliance.Key,
                Title = compliance.Title,
                Details = compliance.Details,
                FilePath = compliance.FilePath,
                ImagePath = compliance.ImagePath,
                //FileType = buyer.FileType,
                CreatedAt = compliance.CreatedAt,
                CreatedBy = compliance.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestCompliance.FilePath;
            string imagePath = RequestCompliance.ImagePath;
            var extention = RequestCompliance.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestCompliance.File != null && !string.IsNullOrEmpty(RequestCompliance.FilePath))
            {
                FileRemove.ImageRemove(RequestCompliance.FilePath);
            }

            if (RequestCompliance.Image != null && !string.IsNullOrEmpty(RequestCompliance.ImagePath))
            {
                FileRemove.ImageRemove(RequestCompliance.ImagePath);
            }

            if (RequestCompliance.File != null)
            {
                //extention = Path.GetExtension(RequestCompliance.File.FileName);
                string message = FileSave.SaveDocument(out filePath, RequestCompliance.File, "Uploads/Compliance");
            }


            if (RequestCompliance.Image != null)
            {
                //extention = Path.GetExtension(RequestCompliance.File.FileName);
                string message = FileSave.SaveImage(out imagePath, RequestCompliance.Image, "Uploads/Compliance");
            }


            Model.Entity.AboutUs.Compliance compliance = await _context.Compliances.FirstOrDefaultAsync(m => m.Id == RequestCompliance.Id);
            compliance.Title = RequestCompliance.Title;
            compliance.Details = RequestCompliance.Details;
            //buyer.FileType = extention;
            compliance.FilePath = filePath;
            compliance.ImagePath = imagePath;
            compliance.UpdatedAt = DateTime.UtcNow;
            compliance.UpdatedBy = user.Email;

            _context.Attach(compliance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplianceExists(compliance.Id))
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


        private bool ComplianceExists(int id)
        {
            return _context.Compliances.Any(e => e.Id == id);
        }
    }
}
