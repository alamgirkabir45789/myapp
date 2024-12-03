using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.EmpLeaveType
{
    public class EmpLeaveTypeRequest
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(200)]
        public string LeaveTypeName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

       
        public double TotalDay { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
