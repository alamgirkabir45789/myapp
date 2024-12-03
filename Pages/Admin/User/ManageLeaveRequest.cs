using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.Pages.Admin.User
{
    public class ManageLeaveRequest
    {
       
        public string LeaveType { get; set; }
     
        //public int LeaveTypeId { get; set; }
        public double TotalLeave { get; set; }

        public string UserId { get; set; }

        public DateTime ValidFromDate { get; set; }

        public DateTime ValidToDate { get; set; }
    }
}
