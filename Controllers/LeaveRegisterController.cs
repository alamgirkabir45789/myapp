using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using myportfolio.Helpers;
using myportfolio.Model;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.LeaveRegister;
using myportfolio.Pages.Admin.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace myportfolio.Controllers
{
   
    
    [Route("api/LeaveRegister")]
    [ApiController]   
    public class LeaveRegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public LeaveRegisterController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpGet]
       

        [Route("GetLeaveRegisters")]
        [HttpGet]        
        public IActionResult GetLeaveRegisters()
        {            
            var record = _context.LeaveRegisters.ToList();
            var modifyLeaveRecord = new List<Model.Entity.LeaveRegister.LeaveRegister>();
            foreach (var item in record)
            {               
                var user = _context.Users.FirstOrDefault(x => x.Id == item.UserId);
                if (user != null)
                {
                    item.EmployeeCode = user.EmployeeCode;
                    modifyLeaveRecord.Add(item);
                }
            }

            return Ok(modifyLeaveRecord);
        }
        
        // GET: api/LeaveRegisters
        [Route("GetLeaveRecordByUserIdAndDateRange")]
        [HttpGet]
        public IActionResult GetLeaveRecordByUserIdAndDateRange(string userId,DateTime fromDate,DateTime toDate)
        {            
            var record = _context.LeaveRegisters.Where(x=>x.UserId==userId && x.IsAccepted==true && x.ApprovedFromDate.Date>=fromDate.Date && x.ApprovedToDate.Date<=toDate.Date && x.ApprovedFromDate.Date<=toDate.Date).ToList();
            return Ok(record);
        }

        [Route("GetLeaveRecordByLeaveType")]
        [HttpGet]
        public async Task<IActionResult> GetLeaveRecordByLeaveType(string userId,string leaveType)
        {
                     
            var validFrom =new DateTime(DateTime.Now.Year,1,1);
            var validTo = new DateTime(DateTime.Now.Year, 12, 31);
            var assignLeave = 0;           
            double consumeLeave = 0;
            var connString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"prcGetCurrentYearAssignLeaveByUserIdAndLeaveType '{userId}','{leaveType}','{validFrom}','{validTo}'", con);
                cmd.ExecuteScalar();
                bool isVal = false;
                SqlDataReader reader =await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    assignLeave= Convert.ToInt32(reader["TotalLeave"]);
                    isVal = true;
                }
                if (isVal)
                {
                    SqlCommand cmd2 = new SqlCommand($"Select  Sum(ApprovedTotalDay) from LeaveRegisters where UserId='{userId}' and LeaveType='{leaveType}' and (SELECT YEAR(ApprovedFromDate))= '{validFrom.Year}'  and (SELECT YEAR(ApprovedToDate))= '{validTo.Year}' group by LeaveType", con);
                    cmd2.ExecuteScalar();
                    SqlDataReader reader2 =await cmd2.ExecuteReaderAsync();

                    if (reader2.Read())
                    {
                        consumeLeave = Convert.ToDouble(reader2.GetValue(0));
                    }
                }         
                con.Close();
            }

            double remainingLeave = 0;
            var totalConsumeLeave = assignLeave - consumeLeave;
            if (totalConsumeLeave <= 0)
            {
                remainingLeave = 0;
            }
            else
            {
                remainingLeave = totalConsumeLeave;
            }
       
            return Ok(remainingLeave);
        }

        // GET: api/LeaveRegisters
        [Route("GetLeaveTotal")]
        [HttpGet]
        public IActionResult GetLeaveTotal(string userId)
        {
            try
            {
                var record = _context.LeaveRegisters.Where(x => x.FromDate.Year == DateTime.Now.Year && x.ToDate.Year == DateTime.Now.Year && x.UserId == userId && x.IsAccepted == true).Select(x => x.ApprovedTotalDay).Sum();
                return Ok(record);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [Route("GetCurrentYearLeaveRecord")]
        [HttpGet]
        public IActionResult GetCurrentYearLeaveRecord(string userId)
        {
            try
            {
                var leaveRecord = _context.LeaveRegisters.Where(x => x.FromDate.Year == DateTime.Now.Year && x.ToDate.Year == DateTime.Now.Year && x.UserId == userId && x.IsAccepted == true).ToList();
                return Ok(leaveRecord);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
     
        [Route("GetCurrentYearLeaveSummary")]
        [HttpGet]
        public IActionResult GetCurrentYearLeaveSummary(string userId)
        {
            List<ManageLeaveRequest> manageLeaveRequests = new List<ManageLeaveRequest>();
            var connString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"prcGetCurrentYearAssignedEmployeeLeaveByUserIdAndValidationDate '{userId}'", con);
                cmd.ExecuteNonQuery();
               
                SqlDataReader reader =  cmd.ExecuteReader();
                while ( reader.Read())
                {
                    var manageLeave = new ManageLeaveRequest()
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
            List<ManageLeaveRegisterRequest> manageLeaveRecord = new List<ManageLeaveRegisterRequest>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"prcGetCurrentYearConsumeLeaveByUserId '{userId}'", con);
                cmd.ExecuteNonQuery();
               
                SqlDataReader reader =  cmd.ExecuteReader();
                while ( reader.Read())
                {
                    var manageLeaveRegister = new ManageLeaveRegisterRequest()
                    {
                        TotalLeave = Convert.ToDouble(reader["TotalLeave"]),
                        UserId = (string)reader["UserId"],
                        LeaveType = (string)reader["LeaveType"],
                        ApprovedFromDate = Convert.ToDateTime(reader["ApprovedFromDate"]),
                        ApprovedToDate = Convert.ToDateTime(reader["ApprovedToDate"])

                    };
                    manageLeaveRecord.Add(manageLeaveRegister);
                }
              
                con.Close();

            }
            List<LeaveSummaryWithLeaveTypeViewModel> list = new List<LeaveSummaryWithLeaveTypeViewModel>();
           
            foreach (var item in manageLeaveRequests)
            {
                
                var leaveRecord = manageLeaveRecord.FirstOrDefault(x=>x.LeaveType == item.LeaveType);
                if(leaveRecord != null)
                {
                    list.Add(new LeaveSummaryWithLeaveTypeViewModel() { LeaveType = item.LeaveType, TotalLeave = item.TotalLeave, RemainingLeave = (item.TotalLeave - leaveRecord.TotalLeave), TakenLeave = leaveRecord.TotalLeave, ValidFromDate = item.ValidFromDate, ValidToDate = item.ValidToDate });
                }
                else
                {
                    list.Add(new LeaveSummaryWithLeaveTypeViewModel() { LeaveType = item.LeaveType, TotalLeave = item.TotalLeave, RemainingLeave = item.TotalLeave, ValidFromDate = item.ValidFromDate, ValidToDate = item.ValidToDate });
                }              
            }

            return Ok(list);
         

        }


        [Route("GetLeaveRecordByDate")]
        [HttpGet]
        public IActionResult GetLeaveRecordByDate(DateTime date)
        {

            try
            {
                var leaveRecord = _context.LeaveRegisters.Where(x => x.FromDate.Date == date && x.IsAccepted==true).ToList();
                return Ok(leaveRecord);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



        [Route("GetApprovedLeaveRegister")]
        [HttpGet]
        public IActionResult GetLeaveRegisterById(bool isAccepted)
        {
            var approvedLeaveRegister = _context.LeaveRegisters.Where(x => x.IsAccepted == isAccepted).ToList();

            return Ok(approvedLeaveRegister);

        }

        [Route("GetRejectedLeaveRegister")]
        [HttpGet]
        public IActionResult GetRejectedLeaveRegister(bool isReject)
        {
            var rejectLeaveRegister = _context.LeaveRegisters.Where(x => x.IsReject == isReject).ToList();

            return Ok(rejectLeaveRegister);

        }

        // GET: api/LeaveRegisters/5
        [Route("GetLeaveRegisterById")]
        [HttpGet]
        public IActionResult GetLeaveRegisterById(int id)
        {
            try
            {
                var leaveRegister = _context.LeaveRegisters.Find(id);
                if (leaveRegister == null)
                {
                    return NotFound("Error");
                }
                return Ok(leaveRegister);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        // PUT: api/LeaveRegisters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("PutLeaveRegister")]
        [HttpPut]
        public IActionResult PutLeaveRegister(LeaveRegister leaveRegister)
        {
            if (leaveRegister == null || leaveRegister.Id == 0)
            {
                if (leaveRegister == null)
                {
                    return BadRequest("Error");
                }
                else if (leaveRegister.Id == 0)
                {
                    return BadRequest($"Invalid Id {leaveRegister.Id}");
                }
            }
            try
            {
                var record = _context.LeaveRegisters.Find(leaveRegister.Id);
                if (record == null)
                {
                    return NotFound("Error");
                }
                record.Name = leaveRegister.Name;
                record.Email = leaveRegister.Email;
                record.ContactNo = leaveRegister.ContactNo;
                record.Designation = leaveRegister.Designation;
                record.Reason = leaveRegister.Reason;
                record.LeaveType = leaveRegister.LeaveType;
                record.IsHours = leaveRegister.IsHours;
                record.IsDays = leaveRegister.IsDays;
                record.IsHalfDay = leaveRegister.IsHalfDay;
                record.FromDate = leaveRegister.FromDate;
                record.HourlyLeaveDate = leaveRegister.HourlyLeaveDate;
                record.ToDate = leaveRegister.ToDate;
                record.FromTime = leaveRegister.FromTime;
                record.ToTime = leaveRegister.ToTime;
                record.ApprovedFromDate = leaveRegister.ApprovedFromDate;
                record.ApprovedToDate = leaveRegister.ApprovedToDate;
                record.TotalDay = leaveRegister.TotalDay;
                record.ApprovedTotalDay = leaveRegister.ApprovedTotalDay;
                record.Comments = leaveRegister.Comments;
                record.IsAccepted = leaveRegister.IsAccepted;
                record.IsReject = leaveRegister.IsReject;
                record.IsChangeDate = leaveRegister.IsChangeDate;
                record.Key = leaveRegister.Key;
                record.Id = leaveRegister.Id;
                record.CreatedAt = leaveRegister.CreatedAt;
                record.CreatedBy = leaveRegister.CreatedBy;
                record.UpdatedAt = DateTime.UtcNow;
                record.UpdatedBy = leaveRegister.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/LeaveRegisters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("PostLeaveRegister")]
        [HttpPost]
        public IActionResult PostLeaveRegister(LeaveRegister leaveRegister)
        {
            try
            {
                leaveRegister.Key = Guid.NewGuid();
                _context.LeaveRegisters.Add(leaveRegister);
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/LeaveRegisters/5
        [Route("DeleteLeaveRegister")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveRegister(int id)
        {
            var leaveRegister = await _context.LeaveRegisters.FindAsync(id);
            if (leaveRegister == null)
            {
                return NotFound();
            }

            _context.LeaveRegisters.Remove(leaveRegister);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
