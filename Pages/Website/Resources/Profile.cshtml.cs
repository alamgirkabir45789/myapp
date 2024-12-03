using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Resources
{
    public class ProfileModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public ProfileModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public Model.Entity.Designes.Slider Slider { get; set; }

        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "RESOURCES").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
