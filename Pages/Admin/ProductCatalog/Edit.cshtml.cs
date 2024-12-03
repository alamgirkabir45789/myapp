using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.ProductCatalog
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
        public Request RequestProductCatalog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Resources.ProductCatalog productCatalog = await _context.ProductCatalogs.FirstOrDefaultAsync(m => m.Id == id);

            if (productCatalog == null)
            {
                return NotFound();
            }

            ViewData["FileGroupId"] = new SelectList(_context.FileGroups.Where(x => x.GroupFor == "catalog"), "Id", "GroupName");

            RequestProductCatalog = new Request()
            {
                Id = productCatalog.Id,
                Key = productCatalog.Key,
                FileName = productCatalog.FileName,
                FilePath = productCatalog.FilePath,
                FileType = productCatalog.FileType,
                CreatedAt = productCatalog.CreatedAt,
                CreatedBy = productCatalog.CreatedBy,
                FileGroupId = productCatalog.FileGroupId
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestProductCatalog.FilePath;
            var extention = RequestProductCatalog.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestProductCatalog.File != null && !string.IsNullOrEmpty(RequestProductCatalog.FilePath))
            {
                FileRemove.ImageRemove(RequestProductCatalog.FilePath);
            }

            if (RequestProductCatalog.File != null)
            {
                extention = Path.GetExtension(RequestProductCatalog.File.FileName);
                string message = FileSave.SaveDocument(out filePath, RequestProductCatalog.File, "Uploads/ProductCatalog");
            }



            Model.Entity.Resources.ProductCatalog productCatalog = await _context.ProductCatalogs.FirstOrDefaultAsync(m => m.Id == RequestProductCatalog.Id);
            productCatalog.FileName = RequestProductCatalog.FileName;
            productCatalog.FileType = extention;
            productCatalog.FilePath = filePath;
            productCatalog.UpdatedAt = DateTime.UtcNow;
            productCatalog.UpdatedBy = user.Email;

            _context.Attach(productCatalog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productCatalogExists(productCatalog.Id))
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




        private bool productCatalogExists(int id)
        {
            return _context.ProductCatalogs.Any(e => e.Id == id);
        }
    }
}
