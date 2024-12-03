using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myportfolio.Model.Entity.LeaveRegister;
using System.Linq;
using System;
using myportfolio.Model.Entity.User;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Habanero.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using myportfolio.Helpers;
using myportfolio.Pages.Admin.User;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace myportfolio.Controllers
{

  
    [Route("api/[controller]")]
    [ApiController]
    public class EmpLeaveController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public EmpLeaveController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var Empleave = _context.EmpLeaves.ToList();
            return Ok(Empleave);
        }

        [Route("GetAllLeaveByUserId")]
        [HttpGet]
        public IActionResult GetAllLeaveByUserId(string userId)
        {
            var Empleave = _context.EmpLeaves.Where(x => x.UserId == userId && x.ValidFromDate.Year == DateTime.Now.Year && x.ValidFromDate.Year == DateTime.Now.Year);


            return Ok(Empleave);
        }

        [Route("GetAssignedEmployeeLeaveByUserIdAndValidationDate")]
        [HttpGet]
        public IActionResult GetAssignedEmployeeLeaveByUserIdAndValidationDate(string userId, DateTime validFrom, DateTime validTo)
        {
            var connString = _configuration.GetConnectionString("DefaultConnection");
            List<ManageLeaveRequest> manageLeaveRequests = new List<ManageLeaveRequest>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"prcGetAssignedEmployeeLeaveByUserIdAndValidationDate '{userId}','{validFrom}','{validTo}'", con);
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var manageLeave = new ManageLeaveRequest
                    {
                        TotalLeave = Convert.ToDouble(reader["TotalLeave"]),
                        UserId = (string)reader["UserId"],
                        LeaveType = (string)reader["LeaveType"],
                        ValidFromDate = Convert.ToDateTime(reader["ValidFromDate"]),
                        ValidToDate = Convert.ToDateTime(reader["ValidToDate"])
                      
                    };
                    manageLeaveRequests.Add(manageLeave);
                }
                con.Close();

            }
            return Ok(manageLeaveRequests);
        }
        [Route("GetEmployeeLeaveByTypeAndDate")]
        [HttpGet]
        public IActionResult GetEmployeeLeaveByTypeAndDate(string userId, string leaveType, DateTime validFrom, DateTime validTo)
        {
           
            var Empleave = _context.EmpLeaves.FromSqlRaw($"prcGetEmployeeLeaveByTypeAndDate '{userId}','{leaveType}','{validFrom}','{validTo}'").ToList();
           

            return Ok(Empleave);
        }

        [Route("GetTotalLeaveByUserIdAndLeaveType")]
        [HttpGet]
        public IActionResult GetTotalLeaveByUserIdAndLeaveType(string userId, string leaveType)
        {
           
            var Empleave = _context.EmpLeaves.FirstOrDefault(x => x.UserId == userId && x.LeaveType == leaveType);
            return Ok(Empleave);
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {

            try
            {
                var leave = _context.EmpLeaves.Find(id);
                if (leave == null)
                {
                    return NotFound("Error");
                }
                return Ok(leave);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [Route("AssignLeaveToMultipleUser")]
        [HttpPost]
        public ActionResult AssignLeaveToMultipleUser(List<EmpLeave> empLeaves)
        {

            if (empLeaves.Count > 0)
            {
                try
                {
                    for (int i = 0; i < empLeaves.Count; i++)
                    {
                        var item = empLeaves[i];
                        if(item != null)
                        {
                            item.Key = Guid.NewGuid();
                            item.LeaveType.ToUpper();
                            _context.AddAsync(item);
                            _context.SaveChanges();
                        }

                    }
                    return Ok("Success");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();

        }


        [Route("CreateEmpLeave")]
        [HttpPost]
        public ActionResult CreateEmpLeave(EmpLeave empLeave)
        {

            try
            {
                empLeave.Key = Guid.NewGuid();
                empLeave.LeaveType.ToUpper();
                _context.AddAsync(empLeave);
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("UpdateEmpLeave")]
        [HttpPut]
        public ActionResult UpdateEmpLeave(EmpLeave empLeave)
        {
            if (empLeave == null || empLeave.Id == 0)
            {
                if (empLeave == null)
                {
                    return BadRequest("Error");
                }
                else if (empLeave.Id == 0)
                {
                    return BadRequest($"Invalid Id {empLeave.Id}");
                }
            }
            try
            {
                var leave = _context.EmpLeaves.Find(empLeave.Id);
                if (leave == null)
                {
                    return NotFound("Error");
                }

                leave.TotalLeave = leave.TotalLeave + empLeave.TotalLeave;
                leave.LeaveType = empLeave.LeaveType.ToUpper();
                leave.LeaveTypeId = empLeave.LeaveTypeId;
                leave.UserId = empLeave.UserId;
                leave.ValidFromDate = empLeave.ValidFromDate;
                leave.ValidToDate = empLeave.ValidToDate;
                leave.Key = empLeave.Key;
                leave.Id = empLeave.Id;
                leave.CreatedAt = empLeave.CreatedAt;
                leave.CreatedBy = empLeave.CreatedBy;
                leave.UpdatedAt = DateTime.UtcNow;
                leave.UpdatedBy = empLeave.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteEmpLeave")]
        [HttpDelete]
        public ActionResult DeleteEmpLeave(int id)
        {

            var leave = _context.EmpLeaves.Find(id);
            if (leave == null)
            {
                return BadRequest("Error");
            }
            _context.EmpLeaves.Remove(leave);
            _context.SaveChanges();
            return Ok("Success");
        }

    }
}
