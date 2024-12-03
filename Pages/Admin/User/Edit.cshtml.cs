using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using myportfolio.ViewModels;
using System.Data;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.User
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
  
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyfService;
        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotyfService notyfService)
        {

            _context = context;
           _userManager = userManager;
            _notyfService = notyfService;
        }

        [BindProperty]             
      public Signup Model { get; set; }
        public async Task <IActionResult> OnGet(string id)
        {
            var user=await _userManager.FindByIdAsync(id);
            ViewData["userId"] = user.Id;
            Signup sn = new Signup()
            {
                Name = user.Name,
                Email = user.Email,
                EmployeeCode= user.EmployeeCode,
                ContactNo = user.ContactNo
            };
            Model = sn;
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
           
                var userData = new Signup()
                {
                    
                    Name = Model.Name,
                    ContactNo = Model.ContactNo,
                    Email = Model.Email,
                    EmployeeCode = Model.EmployeeCode,
                };
                var user =await _userManager.FindByIdAsync(id);
            user.Email = userData.Email;
            user.ContactNo = userData.ContactNo;
            user.Name= userData.Name;
            user.EmployeeCode = userData.EmployeeCode;
            user.UserName = user.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                _notyfService.Success("User has been updated successfully!!!");
                return Redirect("./Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            _notyfService.Error("Something wen wrong!!!");
            return RedirectToPage("Edit", "OnGet");
        }

    }
}
