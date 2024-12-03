using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections;
using myportfolio.Model.Entity.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace myportfolio.Model.Entity.LeaveRegister
{
    public class LeaveRegister:Base
    {
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


        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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
        [NotMapped]
        public string EmployeeCode { get; set; }
        //public virtual ApplicationUser User { get; set; }

    }
}
