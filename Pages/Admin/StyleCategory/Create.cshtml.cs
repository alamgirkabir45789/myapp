using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Model.Entity.User;
using System;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.StyleCategory
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
        public Model.Entity.Styles.StyleCategory StyleCategory { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            StyleCategory.CreatedAt = DateTime.UtcNow;
            StyleCategory.CreatedBy = user.Email;

            await _context.StyleCategories.AddAsync(StyleCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
