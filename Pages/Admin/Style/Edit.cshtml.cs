using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Style
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
        public Request RequestStyle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RequestStyle = new Request();
            RequestStyle.Categories = new List<Model.Entity.Styles.StyleCategory>();
            RequestStyle.Categories = _context.StyleCategories.ToList();
            RequestStyle.Style = await _context.Styles.Include(x => x.StyleCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (RequestStyle.Style == null)
            {
                return NotFound();
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RequestStyle = new Request();
                RequestStyle.Categories = new List<Model.Entity.Styles.StyleCategory>();
                RequestStyle.Categories = _context.StyleCategories.ToList();
                RequestStyle.Style = RequestStyle.Style;
                return Page();
            }

            string filePath = RequestStyle.Style.FilePath;
            var extention = RequestStyle.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestStyle.File != null && !string.IsNullOrEmpty(filePath))
            {
                FileRemove.ImageRemove(filePath);
            }

            if (RequestStyle.File != null)
            {
                extention = Path.GetExtension(RequestStyle.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestStyle.File, "Uploads/Style");
                RequestStyle.Style.FilePath = filePath;
            }


            RequestStyle.Style.UpdatedAt = DateTime.UtcNow;
            RequestStyle.Style.UpdatedBy = user.Email;

            _context.Attach(RequestStyle.Style).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StyleExists(RequestStyle.Style.Id))
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
        private bool StyleExists(int id)
        {
            return _context.Styles.Any(e => e.Id == id);
        }
    }
}
