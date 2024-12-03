using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Threading.Tasks;
using System;

namespace myportfolio.Pages.Website.Subscriber
{
    public class CreateModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public CreateModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OnPostAsync(string email)
        {


            var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            if (ip == "::1")
            {
                ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }

            var subscriber = new Model.Entity.Communication.Subscriber();
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
