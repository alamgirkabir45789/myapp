using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace myportfolio.Pages.Website.Contact
{
    public class ContactUsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ContactUsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }
        public Model.Entity.Designes.Slider Slider { get; set; }


        public IActionResult OnGet()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "CONTACT-US").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            ViewData["Message"] = "";
            return Page();
        }

        [BindProperty]
        public Model.Entity.Communication.Query Query { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            Query.CreatedAt = DateTime.UtcNow;
            Query.CreatedBy = "Normal User";

            await _context.Queries.AddAsync(Query);
            await _context.SaveChangesAsync();
            ViewData["Message"] = "Success";
            //TempData["success"] = "Data Submitted";
            return Page();
        }
    }
}
