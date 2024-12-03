using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace myportfolio.Pages.Admin.ReportedProject
{
    public class ReportedProjectRequested
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(300)]
        public string ProjectName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
