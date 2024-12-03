using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.ProductCatalog
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
            ViewData["FileGroupId"] = new SelectList(_context.FileGroups.Where(x => x.GroupFor == "catalog"), "Id", "GroupName");
            return Page();
        }


        [BindProperty]
        public Request RequestProductCatalog { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestProductCatalog.File.FileName);
            string filePath = string.Empty;

            if (RequestProductCatalog.FileGroupId == null)
            {
                Model.Entity.Shared.FileGroup fileGroup = new Model.Entity.Shared.FileGroup()
                {

                    Key = Guid.NewGuid(),
                    GroupFor = "catalog",
                    GroupName = RequestProductCatalog.FileGroupName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };

                await _context.FileGroups.AddAsync(fileGroup);
                await _context.SaveChangesAsync();
                RequestProductCatalog.FileGroupId = fileGroup.Id;
            }

            string message = FileSave.SaveDocument(out filePath, RequestProductCatalog.File, "Uploads/ProductCatalog");

            Model.Entity.Resources.ProductCatalog productCatalog = new Model.Entity.Resources.ProductCatalog()
            {

                Key = RequestProductCatalog.Key,
                FileName = RequestProductCatalog.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email,
                FileGroupId = (int)RequestProductCatalog.FileGroupId
            };

            await _context.ProductCatalogs.AddAsync(productCatalog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
