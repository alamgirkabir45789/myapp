using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Buyer
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        public DeleteModel(myportfolio.Model.ApplicationDbContext context,INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [BindProperty]
        public Model.Entity.GroupRecources.Buyer Buyer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Buyer = await _context.Buyers.FirstOrDefaultAsync(m => m.Id == id);

            if (Buyer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Buyer = await _context.Buyers.FindAsync(id);

            if (Buyer != null)
            {
                FileRemove.ImageRemove(Buyer.LogoPath);
                _context.Buyers.Remove(Buyer);
               var count= await _context.SaveChangesAsync();
                if(count > 0)
                {
                    _notyfService.Success("Client has been deleted successfully!!!");

                    return RedirectToPage("./Index");
                }
                else
                {
                    _notyfService.Error("Something wen wrong!!!");
                    return RedirectToPage("./Index");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
