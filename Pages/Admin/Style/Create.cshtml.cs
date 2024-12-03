using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myportfolio.Helpers;
using myportfolio.Model.Entity.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.Style
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Request RequestStyle { get; set; }

        public IActionResult OnGet()
        {
            RequestStyle = new Request();
            RequestStyle.Categories = new List<Model.Entity.Styles.StyleCategory>();
            RequestStyle.Categories = _context.StyleCategories.ToList();
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var extention = Path.GetExtension(RequestStyle.File.FileName);
            string filePath = string.Empty;
            string message = FileSave.SaveImage(out filePath, RequestStyle.File, "Uploads/Style");

            Model.Entity.Styles.Style style = new Model.Entity.Styles.Style()
            {
                Key = Guid.NewGuid(),
                StyleNo = RequestStyle.StyleNo,
                Fabrication = RequestStyle.Fabrication,
                Price = RequestStyle.Price,
                PriceFor = RequestStyle.PriceFor,
                ExportTo = RequestStyle.ExportTo,
                Size = RequestStyle.Size,
                Color = RequestStyle.Color,
                Item = RequestStyle.Item,
                StyleCategoryId = RequestStyle.StyleCategoryId,
                FilePath = filePath,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Email
            };

            await _context.Styles.AddAsync(style);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }



    }
}
