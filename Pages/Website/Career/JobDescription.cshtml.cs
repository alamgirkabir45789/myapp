using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myportfolio.Helpers;
using myportfolio.Model.Entity.Career;
using myportfolio.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace myportfolio.Pages.Website.Career
{
    public class JobDescriptionModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public JobDescriptionModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }
        public Model.Entity.Designes.Slider Slider { get; set; }


        [BindProperty]
        public JobCircularViewModel JobCircularViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "CAREER").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            Model.Entity.Career.JobCircular jobCircular = await _context.JobCirculars.FirstOrDefaultAsync(m => m.Key == id);

            if (jobCircular == null)
            {
                return Redirect("/Website/Career/JobCircular");
            }

            JobCircularViewModel = new JobCircularViewModel()
            {
                Id = jobCircular.Id,
                Key = jobCircular.Key,
                JobTitle = jobCircular.JobTitle,
                Overview = jobCircular.Overview,
                Privilege = jobCircular.Privilege,
                StartDate = jobCircular.StartDate,
                EndDate = jobCircular.EndDate,
                NoOfVacancy = jobCircular.NoOfVacancy,
                JobStatus = jobCircular.JobStatus,
                WorkStation = jobCircular.WorkStation,
                Responsibilities = jobCircular.Responsibilities,
                Qualifications = jobCircular.Qualifications,
                Skill = jobCircular.Skill,
                Salary = jobCircular.Salary,
                ResumeViewModel = new ResumeViewModel()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var allowedExtensions = new[] { ".pdf" };

            if (JobCircularViewModel.ResumeViewModel.File.Length > 2097152)
            {
                JobCircularViewModel.ResumeViewModel.Message = "Select pdf less than 2 ΜB.";
                return Page();
            }

            var extention = Path.GetExtension(JobCircularViewModel.ResumeViewModel.File.FileName);
            if (!allowedExtensions.Contains(extention.ToLower()))
            {
                JobCircularViewModel.ResumeViewModel.Message = "File type is not valid.";
                return Page();
            }

            int jobcircularId = JobCircularViewModel.ResumeViewModel.JobCircularId ?? 0;
            Model.Entity.Career.JobCircular jobCircular = await _context.JobCirculars.FirstOrDefaultAsync(m => m.Id == jobcircularId);

            if (jobCircular == null)
            {
                return Redirect("/Website/Career/JobCircular");
            }
            else
            {
                JobCircularViewModel.Id = jobCircular.Id;
                JobCircularViewModel.Key = jobCircular.Key;
                JobCircularViewModel.JobTitle = jobCircular.JobTitle;
                JobCircularViewModel.Overview = jobCircular.Overview;
                JobCircularViewModel.Privilege = jobCircular.Privilege;
                JobCircularViewModel.StartDate = jobCircular.StartDate;
                JobCircularViewModel.EndDate = jobCircular.EndDate;
                JobCircularViewModel.NoOfVacancy = jobCircular.NoOfVacancy;
                JobCircularViewModel.JobStatus = jobCircular.JobStatus;
                JobCircularViewModel.WorkStation = jobCircular.WorkStation;
                JobCircularViewModel.Responsibilities = jobCircular.Responsibilities;
                JobCircularViewModel.Qualifications = jobCircular.Qualifications;
                JobCircularViewModel.Skill = jobCircular.Skill;
                JobCircularViewModel.Salary = jobCircular.Salary;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string filePath = string.Empty;
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(m => m.Phone == JobCircularViewModel.ResumeViewModel.Phone && m.JobCircularId == jobcircularId);

            if (resume != null)
            {
                string message = FileSave.SaveDocument(out filePath, JobCircularViewModel.ResumeViewModel.File, "Uploads/Resume");
                if (message == "success")
                {
                    FileRemove.ImageRemove(resume.FilePath);

                    resume.FullName = JobCircularViewModel.ResumeViewModel.FullName;
                    resume.Email = JobCircularViewModel.ResumeViewModel.Email;
                    resume.YearsOfExperience = JobCircularViewModel.ResumeViewModel.YearsOfExperience;
                    resume.JobCircularId = JobCircularViewModel.ResumeViewModel.JobCircularId;
                    resume.FilePath = filePath;
                    resume.FileType = extention;
                    resume.UpdatedAt = DateTime.UtcNow;

                    _context.Attach(resume).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    JobCircularViewModel.ResumeViewModel.Message = message;
                    return Page();
                }
            }
            else
            {
                string message = FileSave.SaveDocument(out filePath, JobCircularViewModel.ResumeViewModel.File, "Uploads/Resume");
                if (message == "success")
                {
                    resume = new Model.Entity.Career.Resume();

                    resume.Key = Guid.NewGuid();
                    resume.FullName = JobCircularViewModel.ResumeViewModel.FullName;
                    resume.Email = JobCircularViewModel.ResumeViewModel.Email;
                    resume.Phone = JobCircularViewModel.ResumeViewModel.Phone;
                    resume.YearsOfExperience = JobCircularViewModel.ResumeViewModel.YearsOfExperience;
                    resume.JobCircularId = JobCircularViewModel.ResumeViewModel.JobCircularId;
                    resume.FilePath = filePath;
                    resume.FileType = extention;
                    resume.CreatedAt = DateTime.UtcNow;

                    _context.Resumes.Add(resume);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    JobCircularViewModel.ResumeViewModel.Message = message;
                    return Page();
                }
            }

            JobCircularViewModel.ResumeViewModel.FullName = string.Empty;
            JobCircularViewModel.ResumeViewModel.Message = "Your resume submit successful.";
            return Page();
        }

    }
}
