using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.QueryMessage
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Query> Query { get; set; }

        public async Task OnGetAsync()
        {
            Query = await _context.Queries.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int[] queryMessageIds)
        {
            if (queryMessageIds.Count() <= 0)
            {
                return NotFound();
            }

            foreach (var queryMessageId in queryMessageIds)
            {
                var queryMessage = await _context.Queries.FirstOrDefaultAsync(x => x.Id == queryMessageId);
                _context.Queries.Remove(queryMessage);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Success";

            }

            return RedirectToPage("./Index");

        }

    }
}
