using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    public class LeaveRegisterRequest
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserId { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }

        [Required]
        [StringLength(300)]
        public string Reason { get; set; }

        [Required]
        [StringLength(200)]
        public string LeaveType { get; set; }

        [DefaultValue(false)]
        public bool IsHours { get; set; }

        [DefaultValue(false)]
        public bool IsHalfDay { get; set; }

        [DefaultValue(true)]
        public bool IsDays { get; set; }

        public DateTime HourlyLeaveDate { get; set; }


        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; } 

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ApprovedFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ApprovedToDate { get; set; }

        public TimeSpan FromTime { get; set; } 


        public TimeSpan ToTime { get; set; }


        public double TotalDay { get; set; }

        public double ApprovedTotalDay { get; set; }

        public string Comments { get; set; }

        [DefaultValue(false)]
        public bool IsAccepted { get; set; }

        [DefaultValue(false)]
        public bool IsReject { get; set; }

        [DefaultValue(false)]
        public bool IsChangeDate { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
