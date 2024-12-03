using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.User;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace myportfolio.Pages.Admin.Team
{
    [Authorize(Roles = "Admin")]
    public class RetriveModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RetriveModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public Team.Request RequestTeam { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Model.Entity.Team.Team team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            RequestTeam = new Team.Request()
            {
                Id = team.Id,
                Key = team.Key,
                FileName = team.Name,
                SortCode = team.SortCode,
                Email = team.Email,
                ContactNo = team.ContactNo,
                Designation = team.Designation,
                isActive = team.isActive,
                FilePath = team.LogoPath,
                //FileType = buyer.FileType,
                CreatedAt = team.CreatedAt,
                CreatedBy = team.CreatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            string filePath = RequestTeam.FilePath;
            var extention = RequestTeam.FileType;

            var user = await _userManager.GetUserAsync(User);
            if (RequestTeam.File != null && !string.IsNullOrEmpty(RequestTeam.FilePath))
            {
                FileRemove.ImageRemove(RequestTeam.FilePath);
            }

            if (RequestTeam.File != null)
            {
                extention = Path.GetExtension(RequestTeam.File.FileName);
                string message = FileSave.SaveImage(out filePath, RequestTeam.File, "Uploads/Team");
            }



            Model.Entity.Team.Team team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == RequestTeam.Id);
            team.Name = RequestTeam.FileName;
            team.SortCode = RequestTeam.SortCode;
            team.Email = RequestTeam.Email;
            team.ContactNo = RequestTeam.ContactNo;
            team.Designation = RequestTeam.Designation;
            team.isActive = true;
            //buyer.FileType = extention;
            team.LogoPath = filePath;
            team.UpdatedAt = DateTime.UtcNow;
            team.UpdatedBy = user.Email;

            _context.Attach(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(team.Id))
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


        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
