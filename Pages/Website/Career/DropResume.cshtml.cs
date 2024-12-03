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
    public class DropResumeModel : PageModel
    {
        private readonly myportfolio.Model.ApplicationDbContext _context;

        public DropResumeModel(myportfolio.Model.ApplicationDbContext context)
        {
            _context = context;
        }
        public Model.Entity.Designes.Slider Slider { get; set; }


        [BindProperty]
        public ResumeViewModel ResumeViewModel { get; set; }

        public IActionResult OnGet()
        {
            Slider = _context.Sliders.Where(x => x.Identifier == "CAREER").OrderBy(r => Guid.NewGuid()).FirstOrDefault();

            ResumeViewModel = new ResumeViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var allowedExtensions = new[] { ".pdf" };

            if (ResumeViewModel.File.Length > 2097152)
            {
                ResumeViewModel.Message = "Select pdf less than 2 ΜB.";
                return Page();
            }

            var extention = Path.GetExtension(ResumeViewModel.File.FileName);
            if (!allowedExtensions.Contains(extention.ToLower()))
            {
                ResumeViewModel.Message = "File type is not valid.";
                return Page();
            }


            string filePath = string.Empty;
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(m => m.Phone == ResumeViewModel.Phone && m.JobCircularId == null);

            if (resume != null)
            {
                string message = FileSave.SaveDocument(out filePath, ResumeViewModel.File, "Uploads/Resume");
                if (message == "success")
                {
                    FileRemove.ImageRemove(resume.FilePath);

                    resume.FullName = ResumeViewModel.FullName;
                    resume.Email = ResumeViewModel.Email;
                    resume.YearsOfExperience = ResumeViewModel.YearsOfExperience;
                    resume.FilePath = filePath;
                    resume.FileType = extention;
                    resume.UpdatedAt = DateTime.UtcNow;

                    _context.Attach(resume).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ResumeViewModel.Message = message;
                    return Page();
                }
            }
            else
            {
                string message = FileSave.SaveDocument(out filePath, ResumeViewModel.File, "Uploads/Resume");
                if (message == "success")
                {
                    resume = new Model.Entity.Career.Resume();

                    resume.Key = Guid.NewGuid();
                    resume.FullName = ResumeViewModel.FullName;
                    resume.Email = ResumeViewModel.Email;
                    resume.Phone = ResumeViewModel.Phone;
                    resume.YearsOfExperience = ResumeViewModel.YearsOfExperience;
                    resume.FilePath = filePath;
                    resume.FileType = extention;
                    resume.CreatedAt = DateTime.UtcNow;

                    _context.Resumes.Add(resume);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ResumeViewModel.Message = message;
                    return Page();
                }
            }

            ResumeViewModel = new ResumeViewModel();
            ResumeViewModel.Message = "Your resume submit successful.";
            return Page();

        }
    }
}
