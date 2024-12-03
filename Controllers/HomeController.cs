using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Communication;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace myportfolio.Controllers
{
    public class HomeController : Controller
    {

        private readonly myportfolio.Model.ApplicationDbContext _context;
    
        public HomeController(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("test")]       
        public IActionResult Test() => Ok("Hello world");

        #region API
        [Route("global/api/home/subscribe")]
        [HttpGet]
        public Task<IActionResult> Subscribe(string email)
        {
            var isExist=_context.Subscribers.FirstOrDefault(x=>x.Email == email);
            if (isExist == null)
            {
                var ip = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
                if (ip == "::1")
                {
                    ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
                }

                Subscriber subscriber = new Subscriber()
                {
                    Key = Guid.NewGuid(),
                    Email = email,
                    IpAddress = ip,
                    CreatedBy = email,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Subscribers.AddAsync(subscriber);
                _context.SaveChangesAsync();

                return Task.FromResult<IActionResult>(Json(subscriber));
            }
          
            return Task.FromResult<IActionResult>(Json(null));

        }

        [Route("global/api/home/querymessage")]
        [HttpGet]
        public JsonResult DeleteQueryMessage(string id)
        {
            
           if(id == null)
            {
                return Json("sfsdf");

            }
            return Json(new { id });

        }

        #endregion
    }
}
