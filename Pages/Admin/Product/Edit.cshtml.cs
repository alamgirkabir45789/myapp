using AspNetCoreHero.ToastNotification.Abstractions;
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

namespace myportfolio.Pages.Admin.Product
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
        public Request RequestProduct { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Catalogs.Product product = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            RequestProduct = new Request()
            {
                Id = product.Id,
                Key = product.Key,
                Name = product.Name,
                Details = product.Details,
                ProjectUrl = product.ProjectUrl,
                CategoryId = product.CategoryId,
                BusinessType = product.Category.BusinessType,
                FilePath = product.FilePath,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,

            };

            ViewData["Category"] = new SelectList(_context.Categories.Where(x => x.BusinessType == RequestProduct.BusinessType), "Id", "Title");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestProduct.FilePath;
            var extention = RequestProduct.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestProduct.File != null && !string.IsNullOrEmpty(RequestProduct.FilePath))
            {
                FileRemove.ImageRemove(RequestProduct.FilePath);
            }

            if (RequestProduct.File != null)
            {
                extention = Path.GetExtension(RequestProduct.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestProduct.File, "Uploads/Product");
            }

         

            Model.Entity.Catalogs.Product product = await _context.Products.FirstOrDefaultAsync(m => m.Id == RequestProduct.Id);
            product.Name = RequestProduct.Name;
            product.Details = RequestProduct.Details;
            product.ProjectUrl = RequestProduct.ProjectUrl;
            product.CategoryId = RequestProduct.CategoryId;
            product.FilePath = filePath;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = user.Email;

            _context.Attach(product).State = EntityState.Modified;

            try
            {
                var count=await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Project has been updated successfully!!!");
                    return RedirectToPage("./Index");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
