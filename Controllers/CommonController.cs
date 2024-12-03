using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myportfolio.Model.Entity.User;
using System.Linq;

namespace myportfolio.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommonController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        [Route("GetAllDeveloper")]
        [HttpGet]
        public IActionResult GetAllDeveloper()
        {
            var report = _context.Teams.Where(x=>x.isActive).ToList();
            return Ok(report);
        }

        [Route("GetAllProject")]
        [HttpGet]
        public IActionResult GetAllProject()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [Route("GetAllLeaveType")]
        [HttpGet]
        public IActionResult GetAllLeaveType()
        {
            var leaveType = _context.EmpLeaveTypes.ToList();
            return Ok(leaveType);
        }

        [Route("GetAssignProjectByUserName")]
        [HttpGet]
        public IActionResult GetAssignProjectByUserName(string name)
        {
            var assignProjects = _context.DailyReports.Where(x => x.Name == name).ToList();
            return Ok(assignProjects);
        }

        [Route("GetUserDetailsByUserId")]
        [HttpGet]
        public IActionResult GetUserDetailsByUserId(string userId)
        {
            var userDetails = _context.Users.FirstOrDefault(x=>x.Id == userId);
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = userDetails.Email,
                Name = userDetails.Name,
                ContactNo = userDetails.ContactNo,
                EmployeeCode = userDetails.EmployeeCode
            };
            return Ok(applicationUser);
        }
    }
}
