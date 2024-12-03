using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using myportfolio.Model;
using myportfolio.Model.Entity.User;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Role
{
    [Authorize(Roles = "Admin")]
    public class AssignRoleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AssignRoleModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        [BindProperty]
        public AssignRoleRequest AssignRole { get; set; }
        public  void OnGet(string id)
        {
           
            var userList = userManager.Users.ToList();
            ViewData["UserListDdl"] = new SelectList(userList, "Id", "Name");

            var roleList = _roleManager.Roles.ToList();
            ViewData["RoleListDdl"] = new SelectList(roleList, "Id", "Name");
        }



        public async Task<IActionResult> OnPostAsync()
        {

            var userRole = new AssignRoleRequest()
            {
                UserId = AssignRole.UserId,
                RoleId = AssignRole.RoleId
            };

            var user = await userManager.FindByIdAsync(userRole.UserId);
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);

            var result = await userManager.AddToRoleAsync(user, role.Name);


            if (result.Succeeded)
            {

                return Redirect("/Admin/Role/Index");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToPage("./Index");
        }

    }
}