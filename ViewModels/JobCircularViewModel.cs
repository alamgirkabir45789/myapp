using System;

namespace myportfolio.ViewModels
{
    public class JobCircularViewModel
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string JobTitle { get; set; }

        public string Overview { get; set; }

        public string Privilege { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NoOfVacancy { get; set; }

        public string JobStatus { get; set; }

        public string WorkStation { get; set; }

        public string Responsibilities { get; set; }

        public string Qualifications { get; set; }

        public string Skill { get; set; }

        public string Salary { get; set; }

        public ResumeViewModel ResumeViewModel { get; set; }
    }
}
