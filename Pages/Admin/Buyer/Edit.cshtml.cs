using AspNetCoreHero.ToastNotification.Abstractions;
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

namespace myportfolio.Pages.Admin.Buyer
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
        public Buyer.Request RequestBuyer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.GroupRecources.Buyer buyer = await _context.Buyers.FirstOrDefaultAsync(m => m.Id == id);

            if (buyer == null)
            {
                return NotFound();
            }

            RequestBuyer = new Buyer.Request()
            {
                Id = buyer.Id,
                Key = buyer.Key,
                FileName = buyer.Name,
                Description = buyer.Description,
                Profession = buyer.Profession,
                FilePath = buyer.LogoPath,
                //FileType = buyer.FileType,
                CreatedAt = buyer.CreatedAt,
                CreatedBy = buyer.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = RequestBuyer.FilePath;
            var extention = RequestBuyer.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestBuyer.File != null && !string.IsNullOrEmpty(RequestBuyer.FilePath))
            {
                FileRemove.ImageRemove(RequestBuyer.FilePath);
            }

            if (RequestBuyer.File != null)
            {
                extention = Path.GetExtension(RequestBuyer.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestBuyer.File, "Uploads/Buyer");
            }



            Model.Entity.GroupRecources.Buyer buyer = await _context.Buyers.FirstOrDefaultAsync(m => m.Id == RequestBuyer.Id);
            buyer.Name = RequestBuyer.FileName;
            buyer.Description = RequestBuyer.Description;
            buyer.Profession = RequestBuyer.Profession;
            //buyer.FileType = extention;
            buyer.LogoPath = filePath;
            buyer.UpdatedAt = DateTime.UtcNow;
            buyer.UpdatedBy = user.Email;

            _context.Attach(buyer).State = EntityState.Modified;

            try
            {
               var count= await _context.SaveChangesAsync();
                if(count > 0)
                {
                    _notyfService.Success("Client has been updated successfully!!!");

                    return RedirectToPage("./Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerExists(buyer.Id))
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


        private bool BuyerExists(int id)
        {
            return _context.Buyers.Any(e => e.Id == id);
        }
    }
}
