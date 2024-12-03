using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.LeaveRegister
{
    public class EmpLeave:Base
    {
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


    }
}
