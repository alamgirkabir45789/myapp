using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.Pages.Admin.User
{
    public class EmpLeaveRequest
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(200)]
        public string LeaveType { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        public double TotalLeave { get; set; }


        public string UserId { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidToDate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
