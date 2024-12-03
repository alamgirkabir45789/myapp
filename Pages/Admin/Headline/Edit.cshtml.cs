using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Designes;
using myportfolio.Model.Entity.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Headline
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
        public HeadLine HeadLine { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HeadLine = await _context.HeadLines.FirstOrDefaultAsync(m => m.Id == id);

            if (HeadLine == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            HeadLine.UpdatedAt = DateTime.UtcNow;
            HeadLine.UpdatedBy = user.Email;

            _context.Attach(HeadLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeadLineExists(HeadLine.Id))
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

        private bool HeadLineExists(int id)
        {
            return _context.HeadLines.Any(e => e.Id == id);
        }
    }
}
