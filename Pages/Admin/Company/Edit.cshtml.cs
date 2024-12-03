using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Company
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
        public Request RequestCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.GroupRecources.Company company = await _context.Companies.FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            RequestCompany = new Request()
            {
                Id = company.Id,
                Key = company.Key,
                Name = company.Name,
                Description = company.Description,
                History = company.History,
                Established = company.Established,
                LogoPath = company.LogoPath,
                //FileType = buyer.FileType,
                CreatedAt = company.CreatedAt,
                CreatedBy = company.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestCompany.LogoPath;
            var extention = RequestCompany.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestCompany.File != null && !string.IsNullOrEmpty(RequestCompany.LogoPath))
            {
                FileRemove.ImageRemove(RequestCompany.LogoPath);
            }

            if (RequestCompany.File != null)
            {
                extention = Path.GetExtension(RequestCompany.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestCompany.File, "Uploads/Company");
            }



            Model.Entity.GroupRecources.Company company = await _context.Companies.FirstOrDefaultAsync(m => m.Id == RequestCompany.Id);
            company.Name = RequestCompany.Name;
            company.Description = RequestCompany.Description;
            company.History = RequestCompany.History;
            company.Established = RequestCompany.Established;
            //buyer.FileType = extention;
            company.LogoPath = filePath;
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = user.Email;

            _context.Attach(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(company.Id))
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


        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
