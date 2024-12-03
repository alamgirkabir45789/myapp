using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.Company
{
    [Authorize]
    public class AddCompanyImageModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddCompanyImageModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public IActionResult OnGet(int? id)
        {
            return Page();
        }



        [BindProperty]
        public Request RequestCompanyImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestCompanyImage.Image.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestCompanyImage.Image, "Uploads/Company");

            Model.Entity.GroupRecources.CompanyImage companyImage = new Model.Entity.GroupRecources.CompanyImage()
            {
                Key = RequestCompanyImage.Key,
                Title = RequestCompanyImage.Title,
                SortOrder = RequestCompanyImage.SortOrder,
                CompanyId = RequestCompanyImage.CompanyId,
                //FileType = extention,
                ImagePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.CompanyImages.AddAsync(companyImage);
            await _context.SaveChangesAsync();
            return RedirectToPage("./CompanyImage", new { id = RequestCompanyImage.CompanyId });

            //return RedirectToPage("./Index");
        }
    }
}
