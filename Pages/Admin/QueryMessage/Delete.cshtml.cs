using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace myportfolio.Pages.Admin.QueryMessage
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnGetAsync(int[] queryMessageIds)
        {
            ViewData["id"] = queryMessageIds;
        }
    }
}
