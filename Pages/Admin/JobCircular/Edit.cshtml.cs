using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.User;

namespace myportfolio.Pages.Admin.JobCircular
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
        public Model.Entity.Career.JobCircular JobCircular { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobCircular = await _context.JobCirculars.FirstOrDefaultAsync(m => m.Id == id);

            if (JobCircular == null)
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
            JobCircular.UpdatedAt = DateTime.UtcNow;
            JobCircular.UpdatedBy = user.Email;
            
            _context.Attach(JobCircular).State = EntityState.Modified;

            try
            {
               var count= await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Circular has been updated successfully!!!");
                    return RedirectToPage("./Index");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobCircularExists(JobCircular.Id))
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

        private bool JobCircularExists(int id)
        {
            return _context.JobCirculars.Any(e => e.Id == id);
        }
    }
}
