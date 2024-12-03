using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model;
using myportfolio.Model.Entity.Designes;

namespace myportfolio.Pages.Admin.Headline
{
    public class DetailsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DetailsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public HeadLine HeadLine { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HeadLine = await _context.HeadLines.FirstOrDefaultAsync(m => m.Id == id);

            if (HeadLine == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
