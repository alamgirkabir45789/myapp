using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Career
{
    public class JobCircular : Base
    {
        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(5000)]
        public string Overview { get; set; }

        [StringLength(5000)]
        public string Privilege { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NoOfVacancy { get; set; }

        /// <summary>
        /// Part Time, Full Time
        /// </summary>
        [Required]
        [StringLength(50)]
        public string JobStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkStation { get; set; }
        
        [StringLength(5000)]
        public string Responsibilities { get; set; }
        
        [StringLength(5000)]
        public string Qualifications { get; set; }

        [StringLength(5000)]
        public string Skill { get; set; }

        [Required]
        [StringLength(50)]
        public string Salary { get; set; }
    }
}
