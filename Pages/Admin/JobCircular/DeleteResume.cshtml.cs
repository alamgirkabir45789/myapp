using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Career;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.JobCircular
{
    [Authorize(Roles = "Admin")]
    public class DeleteResumeModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DeleteResumeModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Resume Resume { get; set; }



        public async Task<IActionResult> OnPostAsync(int? id, int circularId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Resume = await _context.Resumes.FindAsync(id);

            if (Resume != null)
            {
                FileRemove.ImageRemove(Resume.FilePath);

                _context.Resumes.Remove(Resume);
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./ApplicantList", new { Id = circularId });
        }
    }
}
