using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using myportfolio.Model.Entity.User;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace myportfolio.Pages.Admin.Team
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
                Name = team.Name,
                SortCode = team.SortCode,
                UserId = team.UserId,
                Email = team.Email,
                ContactNo = team.ContactNo,
                Designation = team.Designation,
                Expertise = team.Expertise,
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
                extention = Path.GetExtension(RequestTeam.File.Name);
                string message = FileSave.SaveImage(out filePath, RequestTeam.File, "Uploads/Team");
            }



            Model.Entity.Team.Team team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == RequestTeam.Id);
            team.Name = RequestTeam.Name;
            team.SortCode = RequestTeam.SortCode;
            team.UserId = RequestTeam.UserId;
            team.Email = RequestTeam.Email;
            team.ContactNo = RequestTeam.ContactNo;
            team.Designation = RequestTeam.Designation;
            team.Expertise = RequestTeam.Expertise;
            team.isActive = RequestTeam.isActive;
            //buyer.FileType = extention;
            team.LogoPath = filePath;
            team.UpdatedAt = DateTime.UtcNow;
            team.UpdatedBy = user.Email;

            _context.Attach(team).State = EntityState.Modified;


            try
            {
                var count=await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Team has been updated successfully!!!");
                    return RedirectToPage("./Index");
                }
               
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
