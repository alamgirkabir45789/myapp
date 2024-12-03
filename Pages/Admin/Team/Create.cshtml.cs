using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using myportfolio.Model.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace myportfolio.Pages.Admin.Team
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
            _notyfService = notyfService;
        }

        public IList<ApplicationUser> ApplicationUsers { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var list =await _context.Users.ToListAsync();
            ViewData["List"] =  new SelectList(list, "Name", "Name");
           
            return Page();
        }



        [BindProperty]
        public Team.Request RequestTeam { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestTeam.File.Name);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestTeam.File, "Uploads/Team");
            var selectedUser =await _context.Users.FirstOrDefaultAsync(x => x.Name == RequestTeam.Name);
            var userId=await _userManager.FindByIdAsync(selectedUser.Id);

            Model.Entity.Team.Team team= new Model.Entity.Team.Team()
            {
                Key = RequestTeam.Key,
                Name = RequestTeam.Name,
                SortCode = RequestTeam.SortCode,
                UserId=userId.Id,
                Email = RequestTeam.Email,
                ContactNo = RequestTeam.ContactNo,
                Designation = RequestTeam.Designation,
                Expertise = RequestTeam.Expertise,
                isActive = true,
                //FileType = extention,
                LogoPath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };
            var isExist = _context.Teams.FirstOrDefault(x => x.SortCode == team.SortCode);
            if (isExist == null)
            {
                await _context.Teams.AddAsync(team);
                var count=await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _notyfService.Success("Team has been submitted successfully!!!");

                    return RedirectToPage("./Index");
                }
                else
                {
                    _notyfService.Error("Something wen wrong!!!");
                    return RedirectToPage("Create", "OnGet");
                }
            }
            else
            {
                ViewData["Message"] = "already exists!";
                return Page();
            }
          
        }
    }
}
