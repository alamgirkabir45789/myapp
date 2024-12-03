using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public Request RequestService { get; set; }

       

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var extention = Path.GetExtension(RequestService.File.FileName);
                string filePath = string.Empty;
                string message = FileSave.SaveImage(out filePath, RequestService.File, "Uploads/Service");

                Model.Entity.Catalogs.Category category = new Model.Entity.Catalogs.Category()
                {
                    Key = RequestService.Key,
                    Title = RequestService.Title,
                    Description = RequestService.Description,
                    BusinessType = RequestService.BusinessType,
                    FilePath = filePath,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user.Email
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

            }
            catch (Exception)
            {

                throw;
            }
           

        }
    }
}
