using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;
using myportfolio.Model.Entity.DailyReport;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyReport;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Controllers
{
  
    [Route("api/DailyReport")]
    [ApiController]
   
    public class DailyReportController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DailyReportController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("GetAll")]
        [HttpGet]
        
        public IActionResult GetAll()
        {
            var report = _context.DailyReports.ToList();
            return Ok(report);
        }

       
        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {

            try
            {
                var report = _context.DailyReports.Find(id);
                if (report == null)
                {
                    return NotFound("Error");
                }
                return Ok(report);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [Route("CreateDailyReport")]
        [HttpPost]
        public ActionResult CreateDailyReport(DailyReport dailyReport)
        {
           
            try
            {
                dailyReport.Key = Guid.NewGuid();
                _context.AddAsync(dailyReport);
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateDailyReport")]
        [HttpPut]
        public ActionResult UpdateDailyReport(DailyReport dailyReport)
        {
          
            if (dailyReport == null || dailyReport.Id == 0)
            {
               if(dailyReport == null)
                {
                    return BadRequest("Error");
                }else if(dailyReport.Id == 0)
                {
                    return BadRequest($"Invalid Id {dailyReport.Id}");
                }
            }
            try
            {
                var report = _context.DailyReports.Find(dailyReport.Id);
                if (report == null)
                {
                    return NotFound("Error");
                }
                report.Name = dailyReport.Name;
                report.ProjectName = dailyReport.ProjectName;
                report.AssignDate = dailyReport.AssignDate;
                report.TargetDate = dailyReport.TargetDate;
                report.EndDate = dailyReport.EndDate;
                report.IsCompleted = dailyReport.IsCompleted;
                report.Key = dailyReport.Key;
                report.Id = dailyReport.Id;
                report.CreatedAt = dailyReport.CreatedAt;
                report.CreatedBy = dailyReport.CreatedBy;
                report.UpdatedAt = DateTime.UtcNow;
                report.UpdatedBy = dailyReport.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteDailyReport")]
        [HttpDelete]
        public ActionResult DeleteDailyReport(int id)
        {
            
            var report = _context.DailyReports.Find(id);
            if (report == null)
            {
                return BadRequest("Error");
            }
            _context.DailyReports.Remove(report);
            _context.SaveChanges();
            return Ok("Success");
        }


    }
}
