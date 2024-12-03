using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace myportfolio.Pages.Admin.DailyReport
{
    public class DailyReportRequest
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }


        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string ProjectName { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AssignDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TargetDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
