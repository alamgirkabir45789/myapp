using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.User;
using System;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Category
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
            _notyfService= notyfService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Model.Entity.Catalogs.Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Category.CreatedAt = DateTime.UtcNow;
            Category.CreatedBy = user.Email;

            await _context.Categories.AddAsync(Category);
            var count=await _context.SaveChangesAsync();
            if(count > 0)
            {
                _notyfService.Success("Data has been submitted successfully!!!");
            return RedirectToPage("./Index");
            }
            else
            {
                _notyfService.Error("Something went wrong!!!");
                return RedirectToPage("Create", "OnGet");
            }
        }
    }
}
