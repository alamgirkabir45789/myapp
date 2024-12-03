using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.Service
{
    [Authorize(Roles = "Admin")]
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
        public Request RequestCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Catalogs.Category category = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);


            if (category == null)
            {
                return NotFound();
            }

            RequestCategory = new Request()
            {
                Id = category.Id,
                Key = category.Key,
                Title = category.Title,
                Description = category.Description,
                BusinessType = category.BusinessType,
                FilePath = category.FilePath,
                CreatedAt = category.CreatedAt,
                CreatedBy = category.CreatedBy,

            };

            ViewData["Category"] = new SelectList(_context.Categories.Where(x => x.BusinessType == RequestCategory.BusinessType), "Id", "Title");

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            string filePath = RequestCategory.FilePath;
            var extention = RequestCategory.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestCategory.File != null && !string.IsNullOrEmpty(RequestCategory.FilePath))
            {
                FileRemove.ImageRemove(RequestCategory.FilePath);
            }

            if (RequestCategory.File != null)
            {
                extention = Path.GetExtension(RequestCategory.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestCategory.File, "Uploads/Service");
            }



            Model.Entity.Catalogs.Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == RequestCategory.Id);
            category.Title = RequestCategory.Title;
            category.Description = RequestCategory.Description;
            category.FilePath = filePath;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = user.Email;

            _context.Attach(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
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
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
