using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using myportfolio.Model.Entity.User;
using myportfolio.Model.Entity.ReportedProject;
using Microsoft.AspNetCore.Authorization;

namespace myportfolio.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class ReportedProjectController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportedProjectController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var reportedProjects = _context.ReportedProjects.ToList();
            return Ok(reportedProjects);
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {

            try
            {
                var reportedProject = _context.ReportedProjects.Find(id);
                if (reportedProject == null)
                {
                    return NotFound("Error");
                }
                return Ok(reportedProject);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [Route("CreateReportedProject")]
        [HttpPost]
        public ActionResult CreateReportedProject(ReportedProject reportedProject)
        {
           
            try
            {
                reportedProject.Key = Guid.NewGuid();
                _context.AddAsync(reportedProject);
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateReportedProject")]
        [HttpPut]
        public ActionResult UpdateReportedProject(ReportedProject reportedProject)
        {
           
            if (reportedProject == null || reportedProject.Id == 0)
            {
                if (reportedProject == null)
                {
                    return BadRequest("Error");
                }
                else if (reportedProject.Id == 0)
                {
                    return BadRequest($"Invalid Id {reportedProject.Id}");
                }
            }
            try
            {
                var reportedProj = _context.ReportedProjects.Find(reportedProject.Id);
                if (reportedProj == null)
                {
                    return NotFound("Error");
                }
                reportedProj.ProjectName = reportedProject.ProjectName;
                reportedProj.Description = reportedProject.Description;
                reportedProj.IsCompleted = reportedProject.IsCompleted;

                reportedProj.CreatedAt = reportedProject.CreatedAt;
                reportedProj.CreatedBy = reportedProject.CreatedBy;
                reportedProj.UpdatedAt = DateTime.UtcNow;
                reportedProj.UpdatedBy = reportedProject.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteReportedProject")]
        [HttpDelete]
        public ActionResult DeleteReportedProject(int id)
        {
           
            var reportedProject = _context.ReportedProjects.Find(id);
            if (reportedProject == null)
            {
                return BadRequest("Error");
            }
            _context.ReportedProjects.Remove(reportedProject);
            _context.SaveChanges();
            return Ok("Success");
        }
    }
}
