using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using myportfolio.Helpers;
using System.IO;
using System.Threading.Tasks;
using System;
using myportfolio.Model.Entity.Communication;
using myportfolio.Model.Entity.AboutUs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace myportfolio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public IndexModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

     
        public async Task<IActionResult> OnPostAsync(string email)
        {
            

            var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            if(ip == "::1")
            {
                ip=Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }

            var subscriber=new Model.Entity.Communication.Subscriber();
            subscriber.Email = email;
            subscriber.IpAddress = ip;
            subscriber.CreatedBy = email;
            subscriber.Key = Guid.NewGuid(); ;
            subscriber.CreatedAt = DateTime.UtcNow;

            await _context.Subscribers.AddAsync(subscriber);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
