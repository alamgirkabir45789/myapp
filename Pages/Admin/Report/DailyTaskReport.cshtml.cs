using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myportfolio.Pages.Admin.Report
{
    [Authorize(Roles = "Admin")]
    public class DailyTaskReportModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
