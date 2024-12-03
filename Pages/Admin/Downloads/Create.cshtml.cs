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

namespace myportfolio.Pages.Admin.Downloads
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
            ViewData["FileGroupId"] = new SelectList(_context.FileGroups.Where(x => x.GroupFor == "download"), "Id", "GroupName");
            return Page();
        }


        [BindProperty]
        public Request RequestDownloads { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestDownloads.File.FileName);
            string filePath = string.Empty;

            if (RequestDownloads.FileGroupId == null)
            {
                Model.Entity.Shared.FileGroup fileGroup = new Model.Entity.Shared.FileGroup()
                {

                    Key = Guid.NewGuid(),
                    GroupFor = "download",
                    GroupName = RequestDownloads.FileGroupName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };

                await _context.FileGroups.AddAsync(fileGroup);
                await _context.SaveChangesAsync();
                RequestDownloads.FileGroupId = fileGroup.Id;
            }

            string message = FileSave.SaveDocument(out filePath, RequestDownloads.File, "Uploads/Downloads");

            Model.Entity.Resources.Download download = new Model.Entity.Resources.Download()
            {

                Key = RequestDownloads.Key,
                FileName = RequestDownloads.FileName,
                FileType = extention,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email,
                FileGroupId = (int)RequestDownloads.FileGroupId
            };

            await _context.Downloads.AddAsync(download);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
