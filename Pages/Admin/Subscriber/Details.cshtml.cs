using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.User;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.Subscriber
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

        public Model.Entity.Communication.Subscriber Subscriber { get; set; }

        public async Task<IActionResult> OnGetAsync(string isRead, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriber = await _context.Subscribers.FirstOrDefaultAsync(m => m.Id == id);

            if (Subscriber == null)
            {
                return NotFound();
            }

            if (isRead == "unread")
            {
                var user = await _userManager.GetUserAsync(User);
                Subscriber.IsResponse = true;
                Subscriber.UpdatedAt = DateTime.UtcNow;
                Subscriber.UpdatedBy = user.Email;
                _context.Attach(Subscriber).State = EntityState.Modified;

                await _context.SaveChangesAsync();

            }

            return Page();
        }
    }
}
