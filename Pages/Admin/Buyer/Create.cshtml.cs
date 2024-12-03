using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.IO;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Buyer
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;
        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager,INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }



        [BindProperty]
        public Request RequestBuyer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestBuyer.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestBuyer.File, "Uploads/Buyer");

            Model.Entity.GroupRecources.Buyer buyer = new Model.Entity.GroupRecources.Buyer()
            {
                Key = RequestBuyer.Key,
                Name = RequestBuyer.FileName,
                Description = RequestBuyer.Description,
                Profession = RequestBuyer.Profession,
                //FileType = extention,
                LogoPath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Buyers.AddAsync(buyer);
           var count= await _context.SaveChangesAsync();
            if(count > 0)
            {
                _notyfService.Success("Client has been submitted successfully!!!");

                return RedirectToPage("./Index");
            }
            else
            {
                _notyfService.Error("Something went Wrong!!!");
                return RedirectToPage("Create", "OnGet");
            }

        }
    }
}
