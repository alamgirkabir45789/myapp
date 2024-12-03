using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.DailyReport;
using myportfolio.Model.Entity.DailyTask;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.DailyTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ApplicationUser = myportfolio.Model.Entity.User.ApplicationUser;
using DailyTask = myportfolio.Model.Entity.DailyTask.DailyTask;

namespace myportfolio.Controllers
{
    
    [Route("api/DailyTask")]
    [ApiController]
    public class DailyTaskController : ControllerBase
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DailyTaskController(myportfolio.Model.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {

            var tasks = _context.DailyTasks.ToList();
            var modifyTask = new List<Model.Entity.DailyTask.DailyTask>();
            foreach (var item in tasks)
            {               
                var user = _context.Users.FirstOrDefault(x => x.Id == item.UserId);               
                if (item.WorkingProject !="")
                {
                    string project = item.WorkingProject;
                    string[] projects = project.Split(',');
                    int[] myInts = projects.Select(int.Parse).ToArray();
                    var workingProject = "";
                    for (int i = 0; i < myInts.Length; i++)
                    {
                        int ids = Convert.ToInt32(myInts[i]);
                        if (ids != 0)
                        {
                            var prName = _context.ReportedProjects.FirstOrDefault(x => x.Id == ids);
                            if (prName != null)
                            {
                                workingProject += prName.ProjectName + ",";
                            }
                        }
                    }
                    var modifiedWorkingProjectId = workingProject.Remove(workingProject.Length - 1);
                    item.WorkingProject = modifiedWorkingProjectId;
                    item.SortCode = Convert.ToInt32(user.EmployeeCode);
                    modifyTask.Add(item);
                }
                else
                {
                    item.SortCode = Convert.ToInt32(user.EmployeeCode);
                    modifyTask.Add(item);
                }
            }         

            return Ok(modifyTask);
        }


        [Route("GetMonthlyTaskReportByUserId")]
        [HttpGet]
        public IActionResult GetMonthlyTaskReportByUserId(string userId, int month, int year)
        {
            var tasks = _context.DailyTasks.Where(x => x.UserId == userId && x.SubmitDate.Month == month && x.SubmitDate.Year == year).ToList();
            int totalDaysInSelectedMonth = DateTime.DaysInMonth(year, month);
           
            var modifyTask = new List<TaskReportViewModel>();
            for (int i = 0; i < totalDaysInSelectedMonth; i++)
            {
                int num = i + 1;
                var date = new DateTime(year, month, num).ToString("MM/dd/yyyy");
                var task = tasks.FirstOrDefault(x => x.SubmitDate.Date == Convert.ToDateTime(date));
                var leaveUsers = _context.LeaveRegisters.FirstOrDefault(x=>x.ApprovedFromDate == Convert.ToDateTime(date) && x.IsAccepted==true && x.UserId==userId);

                var dayName = Convert.ToDateTime(date).ToString("dddd");
                var name = _context.Users.FirstOrDefault(x => x.Id == userId).Name;

                if (task != null)
                {

                    if(task.WorkingProject != "")
                    {
                        string project = task.WorkingProject;
                        string[] projects = project.Split(',');
                        int[] myInts = projects.Select(int.Parse).ToArray();
                        var workingProject = "";
                        for (int k = 0; k < myInts.Length; k++)
                        {
                            int ids = Convert.ToInt32(myInts[k]);
                            if (ids != 0)
                            {
                                var prName = _context.ReportedProjects.FirstOrDefault(x => x.Id == ids);
                                if (prName != null)
                                {
                                    workingProject += prName.ProjectName + ",";
                                }
                            }
                        }
                        var modifiedWorkingProjectId = workingProject.Remove(workingProject.Length - 1);
                        task.WorkingProject = modifiedWorkingProjectId;
                    }
                    modifyTask.Add(new TaskReportViewModel()
                    {
                        Name = task.Name,
                        UserId = task.UserId,
                        Description = task.Description,
                        SubmitDate = task.SubmitDate,
                        InTime = task.InTime,
                        OutTime = task.OutTime,
                        WorkingProject = task.WorkingProject,
                        IsHoliday= task.IsHoliday,
                        Key = task.Key,
                        Id = task.Id,
                        IsInLeave = false,
                        IsHalfDayLeave = false

                    });

                }               
                else if (leaveUsers !=null  && task == null)
                {
                    if (leaveUsers.IsHalfDay)
                    {
                        modifyTask.Add(new TaskReportViewModel()
                        {
                            SubmitDate = Convert.ToDateTime(date),
                            Name = name,
                            IsInLeave = false,
                            IsHalfDayLeave = true,
                            Reason = leaveUsers.Reason,
                        });
                    }
                    else
                    {
                        modifyTask.Add(new TaskReportViewModel()
                        {
                            SubmitDate = Convert.ToDateTime(date),
                            Name = name,
                            IsInLeave = true,
                            IsHalfDayLeave = false,
                            Reason = leaveUsers.Reason,
                        });
                    }

                }
                else
                {
                    modifyTask.Add(new TaskReportViewModel() { SubmitDate = Convert.ToDateTime(date), Name = name, });
                }
            }        
            return Ok(modifyTask);
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                var task = _context.DailyTasks.Find(id);
                if(task.WorkingProject != "")
                {
                    string project = task.WorkingProject;
                    string[] projects = project.Split(',');
                    int[] myInts = projects.Select(int.Parse).ToArray();
                    var workingProject = "";

                    for (int i = 0; i < myInts.Length; i++)
                    {
                        int ids = Convert.ToInt32(myInts[i]);
                        if (ids != 0)
                        {
                            var prName = _context.ReportedProjects.FirstOrDefault(x => x.Id == ids);
                            if (prName != null)
                            {
                                workingProject += prName.ProjectName + ",";
                            }
                        }
                    }
                    var modifiedWorkingProjectId = workingProject.Remove(workingProject.Length - 1);
                    task.WorkingProject = modifiedWorkingProjectId;
                }
                if (task == null)
                {
                    return NotFound("Error");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [Route("TaskReportByDate")]
        [HttpGet]
        public IActionResult TaskReportByDate(DateTime date)
        {
            try
            {
                var task = _context.DailyTasks.Where(x => x.SubmitDate == date).ToList();
                var users = (from p in _context.Users
                             join ps in _context.UserRoles on p.Id equals ps.UserId
                             join rs in _context.Roles on ps.RoleId equals rs.Id
                             where rs.Name == "Employee"
                             select new
                             {
                                 p.Id,
                                 p.Name,
                                 p.Email,
                                 p.ContactNo
                             }
                          ).ToList();
                var leaveUsers = _context.LeaveRegisters
                  .FromSql($"prcGetLeaveUserByDate {date}")
                      .ToList();
                if (task == null)
                {
                    return NotFound("Error");
                }
                var taskReports = new List<TaskReportViewModel>();

                foreach (var item in users)
                {
                    var tas = task.FirstOrDefault(x => x.UserId == item.Id);
                    var user = _context.Users.FirstOrDefault(x => x.Id == item.Id);

                    if (tas == null)
                    {
                        taskReports.Add(new TaskReportViewModel { WorkingProject = null, Name = item.Name, Description = null, UserId = item.Id, SortCode = Convert.ToInt32(user.EmployeeCode) });
                    }
                    else
                    {
                       if(tas.WorkingProject != "")
                        {
                            string project = tas.WorkingProject;
                            string[] projects = project.Split(',');
                            int[] myInts = projects.Select(int.Parse).ToArray();
                            var workingProject = "";
                            for (int i = 0; i < myInts.Length; i++)
                            {
                                int ids = Convert.ToInt32(myInts[i]);
                                if (ids != 0)
                                {
                                    var prName = _context.ReportedProjects.FirstOrDefault(x => x.Id == ids);
                                    if (prName != null)
                                    {
                                        workingProject += prName.ProjectName + ",";
                                    }
                                }
                            }
                            var modifiedWorkingProjectId = workingProject.Remove(workingProject.Length - 1);
                            tas.WorkingProject = modifiedWorkingProjectId;

                            taskReports.Add(new TaskReportViewModel { WorkingProject = tas.WorkingProject, Name = item.Name, UserId = item.Id, Id = tas.Id, InTime = tas.InTime, OutTime = tas.OutTime, Description = tas.Description, SubmitDate = tas.SubmitDate,IsHoliday=tas.IsHoliday, SortCode = Convert.ToInt32(user.EmployeeCode) });
                        }
                        else
                        {
                            taskReports.Add(new TaskReportViewModel { WorkingProject = tas.WorkingProject, Name = item.Name, UserId = item.Id, Id = tas.Id, InTime = tas.InTime, OutTime = tas.OutTime, Description = tas.Description, SubmitDate = tas.SubmitDate,IsHoliday=tas.IsHoliday, SortCode = Convert.ToInt32(user.EmployeeCode) });
                        }
                    }

                }

                foreach (var item in taskReports)
                {
                    var lUser = leaveUsers.FirstOrDefault(x => x.UserId == item.UserId);
                    if (lUser == null)
                    {
                        item.Name = item.Name;
                        item.Description = item.Description;
                        item.InTime = item.InTime;
                        item.OutTime = item.OutTime;
                        item.WorkingProject = item.WorkingProject;
                        item.IsHoliday= item.IsHoliday;
                        item.Id = item.Id;
                        item.IsInLeave = false;
                        item.IsHalfDayLeave = false;
                    }
                    else
                    {
                        if (lUser.IsHalfDay == true)
                        {
                            item.Name = item.Name;
                            item.Description = item.Description;
                            item.InTime = item.InTime;
                            item.OutTime = item.OutTime;
                            item.WorkingProject = item.WorkingProject;
                            item.IsHoliday = item.IsHoliday;
                            item.Id = item.Id;
                            item.IsInLeave = false;
                            item.IsHalfDayLeave = true;
                            item.FromDate = lUser.FromDate;
                            item.ToDate = lUser.ToDate;
                            item.Reason = lUser.Reason;
                        }
                        else
                        {
                            item.Name = item.Name;
                            item.Description = item.Description;
                            item.InTime = item.InTime;
                            item.OutTime = item.OutTime;
                            item.WorkingProject = item.WorkingProject;
                            item.IsHoliday = item.IsHoliday;
                            item.Id = item.Id;
                            item.IsInLeave = true;
                            item.IsHalfDayLeave = false;
                            item.FromDate = lUser.FromDate;
                            item.ToDate = lUser.ToDate;
                            item.Reason = lUser.Reason;
                        }

                    }
                }



                var modifiedTaskReport = taskReports.OrderBy(x => x.SortCode).ToList();

                return Ok(modifiedTaskReport);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }


        [Route("CreateDailyTask")]
        [HttpPost]
        public ActionResult CreateDailyTask(DailyTask dailyTask)
        {
            var todaysTask = _context.DailyTasks.FirstOrDefault(x => x.SubmitDate.Date == dailyTask.SubmitDate.Date && x.UserId == dailyTask.UserId);  

            if (todaysTask == null)
            {
                try
                {
                    dailyTask.Key = Guid.NewGuid();
                    _context.AddAsync(dailyTask);
                    _context.SaveChanges();
                    return Ok("Success");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            if (todaysTask != null)
            {
                return BadRequest("Task Already Subitted.");
            }
            return BadRequest();

        }

        [Route("CreateOrUpdateMultiDailyTask")]
        [HttpPost]
        public ActionResult CreateOrUpdateMultiDailyTask(List<DailyTask> dailyTasks)
        {           

            if (dailyTasks.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dailyTasks.Count; i++)
                    {
                        var item = dailyTasks[i];
                        var todaysTask = _context.DailyTasks.FirstOrDefault(x => x.SubmitDate.Date == item.SubmitDate.Date && x.UserId == item.UserId);
                        if (todaysTask == null)
                        {
                            item.Key = Guid.NewGuid();
                            _context.Add(item);
                            _context.SaveChanges();
                        }
                        else
                        {
                            if (todaysTask.IsHoliday)
                            {
                                _context.DailyTasks.Remove(todaysTask);
                                _context.SaveChanges();
                            }
                            else
                            {
                            return BadRequest();
                            }
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


        [Route("UpdateDailyTask")]
        [HttpPut]
        public ActionResult UpdateDailyTask(DailyTask dailyTask)
        {
            
            if (dailyTask == null || dailyTask.Id == 0)
            {
                if (dailyTask == null)
                {
                    return BadRequest("Error");
                }
                else if (dailyTask.Id == 0)
                {
                    return BadRequest($"Invalid Id {dailyTask.Id}");
                }
            }
            try
            {
                var task = _context.DailyTasks.Find(dailyTask.Id);
                if (task == null)
                {
                    return NotFound("Error");
                }
                task.Name = dailyTask.Name;
                task.UserId = dailyTask.UserId;
                task.Description = dailyTask.Description;
                task.SubmitDate = dailyTask.SubmitDate;
                task.InTime = dailyTask.InTime;
                task.OutTime = dailyTask.OutTime;
                task.WorkingProject = dailyTask.WorkingProject;
                task.IsHoliday=dailyTask.IsHoliday;
                task.Key = dailyTask.Key;
                task.Id = dailyTask.Id;
                task.CreatedAt = dailyTask.CreatedAt;
                task.CreatedBy = dailyTask.CreatedBy;
                task.UpdatedAt = DateTime.UtcNow;
                task.UpdatedBy = dailyTask.UpdatedBy;
                _context.SaveChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteDailyTask")]
        [HttpDelete]
        public ActionResult DeleteDailyTask(int id)
        {
           
            var task = _context.DailyTasks.Find(id);
            if (task == null)
            {
                return BadRequest("Error");
            }
            _context.DailyTasks.Remove(task);
            _context.SaveChanges();
            return Ok("Success");
        }

    }
}
