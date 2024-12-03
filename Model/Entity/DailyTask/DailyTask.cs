using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using myportfolio.Model.Entity.Catalogs;
using System.ComponentModel.DataAnnotations.Schema;
using myportfolio.Model.Entity.User;

namespace myportfolio.Model.Entity.DailyTask
{
    public class DailyTask:Base
    {
        //[ForeignKey("ApplicationUser")]
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string UserId { get; set; }
        [NotMapped]
        public int SortCode { get; set; }
       
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SubmitDate { get; set; }

        public TimeSpan InTime { get; set; }


        public TimeSpan OutTime { get; set; }

        public string WorkingProject { get; set; }

        [DefaultValue(false)]
        public bool IsHoliday { get; set; }
        //public virtual ApplicationUser User { get; set; }
        //public int DailyReportId { get; set; }
        //public Model.Entity.DailyReport.DailyReport DailyReport { get; set; }
    }
}
