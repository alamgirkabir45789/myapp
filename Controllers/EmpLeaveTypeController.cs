using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using myportfolio.Model.Entity.User;
using myportfolio.Model.Entity.LeaveRegister;
using Microsoft.AspNetCore.Authorization;

namespace myportfolio.Controllers
{

  
    [Route("api/[controller]")]
    [ApiController]
    public class EmpLeaveTypeController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpLeaveTypeController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var leaveTypes = _context.EmpLeaveTypes.ToList();
            return Ok(leaveTypes);
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {

            try
            {
                var leaveTypes = _context.EmpLeaveTypes.Find(id);
                if (leaveTypes == null)
                {
                    return NotFound("Error");
                }
                return Ok(leaveTypes);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [Route("CreateEmpLeaveType")]
        [HttpPost]
        public ActionResult CreateEmpLeaveType(EmpLeaveType empLeaveType)
        {
            
            var isExist = _context.EmpLeaveTypes.FirstOrDefault(x => x.LeaveTypeName == empLeaveType.LeaveTypeName);
            if (isExist == null)
            {
                try
                {
                    empLeaveType.Key = Guid.NewGuid();
                    empLeaveType.LeaveTypeName.ToUpper();
                    _context.AddAsync(empLeaveType);
                    _context.SaveChanges();
                    return Ok("Success");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [Route("UpdateEmpLeaveType")]
        [HttpPut]
        public ActionResult UpdateEmpLeaveType(EmpLeaveType empLeaveType)
        {
           
            if (empLeaveType == null || empLeaveType.Id == 0)
            {
                if (empLeaveType == null)
                {
                    return BadRequest("Error");
                }
                else if (empLeaveType.Id == 0)
                {
                    return BadRequest($"Invalid Id {empLeaveType.Id}");
                }
            }
            try
            {
                var leaveType = _context.EmpLeaveTypes.Find(empLeaveType.Id);
                if (leaveType == null)
                {
                    return NotFound("Error");
                }
                
                leaveType.TotalDay = empLeaveType.TotalDay;
                leaveType.LeaveTypeName= empLeaveType.LeaveTypeName.ToUpper();
                leaveType.Description = empLeaveType.Description;
                leaveType.Key = empLeaveType.Key;
                leaveType.Id = empLeaveType.Id;
                leaveType.CreatedAt = empLeaveType.CreatedAt;
                leaveType.CreatedBy = empLeaveType.CreatedBy;
                leaveType.UpdatedAt = DateTime.UtcNow;
                leaveType.UpdatedBy = empLeaveType.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteEmpLeaveType")]
        [HttpDelete]
        public ActionResult DeleteEmpLeaveType(int id)
        {
           
            var leaveType = _context.EmpLeaveTypes.Find(id);
            if (leaveType == null)
            {
                return BadRequest("Error");
            }
            _context.EmpLeaveTypes.Remove(leaveType);
            _context.SaveChanges();
            return Ok("Success");
        }

    }
}
