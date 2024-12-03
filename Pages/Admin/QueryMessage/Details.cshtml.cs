using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Communication;
using myportfolio.Model.Entity.User;
using System;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.QueryMessage
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Query Query { get; set; }

        public async Task<IActionResult> OnGetAsync(string isRead, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Query = await _context.Queries.FirstOrDefaultAsync(m => m.Id == id);

            if (Query == null)
            {
                return NotFound();
            }

            if (isRead == "unread")
            {
                var user = await _userManager.GetUserAsync(User);
                Query.IsRead = true;
                Query.UpdatedAt = DateTime.UtcNow;
                Query.UpdatedBy = user.Email;
                _context.Attach(Query).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }

            return Page();
        }
    }
}
