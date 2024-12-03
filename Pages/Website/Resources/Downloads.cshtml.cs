using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.Resources;
using myportfolio.Model.Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Resources
{
    public class DownloadsModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DownloadsModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FileGroup> FileGroups { get; set; }
        public IList<Download> Downloads { get; set; }
        public Model.Entity.Designes.Slider Slider { get; set; }
        public async Task OnGetAsync()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "RESOURCES").OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            var groupIds = _context.Downloads.Select(x => x.FileGroupId).Distinct().ToList();
            FileGroups = await _context.FileGroups.Where(x => groupIds.Contains(x.Id)).ToListAsync();
            Downloads = await _context.Downloads.ToListAsync();
        }
    }
}
