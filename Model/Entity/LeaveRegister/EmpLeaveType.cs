using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.LeaveRegister
{
    public class EmpLeaveType:Base
    {
       
        [Required]
        [StringLength(200)]
        public string LeaveTypeName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

       
        public double TotalDay { get; set; }
    }
}
