using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace myportfolio.Pages.Website.Style
{
    public class StyleDetailModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public StyleDetailModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.Styles.Style Style { get; set; }

        public async Task OnGetAsync(Guid key)
        {
            Style = await _context.Styles.Include(x=>x.StyleCategory).FirstOrDefaultAsync(x => x.Key == key);
        }
    }
}
