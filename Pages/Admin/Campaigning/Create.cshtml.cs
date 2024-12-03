using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.Campaigning
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
        public Model.Entity.Catalogs.Campaigning Campaigning { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Campaigning.CreatedAt = DateTime.UtcNow;
            Campaigning.CreatedBy = user.Email;

            await _context.Campaignings.AddAsync(Campaigning);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
