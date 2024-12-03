using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Company
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
        public Request RequestCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestCompany.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestCompany.File, "Uploads/Company");

            Model.Entity.GroupRecources.Company company = new Model.Entity.GroupRecources.Company()
            {
                Key = RequestCompany.Key,
                Name = RequestCompany.Name,
                Description = RequestCompany.Description,
                History = RequestCompany.History,
                Established = RequestCompany.Established,
                //FileType = extention,
                LogoPath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
