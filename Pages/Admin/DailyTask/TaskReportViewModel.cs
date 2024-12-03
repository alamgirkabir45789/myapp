using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.Pages.Admin.DailyTask
{
    public class TaskReportViewModel
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public int SortCode { get; set; }

       
        public string Description { get; set; }
        public bool IsInLeave { get; set; }

        public bool IsHalfDayLeave { get; set; }

       
        public string Reason { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SubmitDate { get; set; }

        public TimeSpan InTime { get; set; }


        public TimeSpan OutTime { get; set; }

        public string WorkingProject { get; set; }
        public bool IsHoliday { get; set; }
        //public int DailyReportId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
