using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Product
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

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return Page();
        }


        [BindProperty]
        public Request RequestProduct { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestProduct.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestProduct.File, "Uploads/Product");

            Model.Entity.Catalogs.Product product = new Model.Entity.Catalogs.Product()
            {
                Key = RequestProduct.Key,
                Name = RequestProduct.Name,
                Details = RequestProduct.Details,
                ProjectUrl = RequestProduct.ProjectUrl,
                CategoryId = RequestProduct.CategoryId,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Products.AddAsync(product);
            var count=await _context.SaveChangesAsync();
            if (count > 0)
            {
                _notyfService.Success("Project has been created successfully!!!");
                return RedirectToPage("./Index");

            }
            return RedirectToPage("./Index");
        }



    }
}
