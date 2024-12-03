using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myportfolio.Model.Entity.User;
using System.Linq;
using System;
using myportfolio.Model.Entity.Holiday;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace myportfolio.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HolidayController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var hDays = _context.Holidays.ToList();
            return Ok(hDays);
        }

        [Route("CreateHoliday")]
        [HttpPost]
        public ActionResult CreateHoliday(Holiday holiday )
        {
            var hDay = _context.Holidays.FirstOrDefault(x => x.HEventDate.Date == holiday.HEventDate.Date );
            if (hDay == null)
            {
                try
                {
                    holiday.Key = Guid.NewGuid();
                    _context.AddAsync(holiday);
                    _context.SaveChanges();
                    return Ok("Success");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            if (hDay != null)
            {
                return BadRequest("Holiday Already Subitted.");
            }
            return BadRequest();

        }

    }
}
