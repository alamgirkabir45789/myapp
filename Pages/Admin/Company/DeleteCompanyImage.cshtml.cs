using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.GroupRecources;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Company
{
    [Authorize]
    public class DeleteCompanyImageModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DeleteCompanyImageModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CompanyImage CompanyImage { get; set; }



        public async Task<IActionResult> OnPostAsync(int? companyImgId, int companyId)
        {
            if (companyImgId == null)
            {
                return NotFound();
            }

            CompanyImage = await _context.CompanyImages.FindAsync(companyImgId);

            if (CompanyImage != null)
            {
                FileRemove.ImageRemove(CompanyImage.ImagePath);

                _context.CompanyImages.Remove(CompanyImage);
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./CompanyImage", new { id = companyId });
        }
    }
}
