using System;

namespace myportfolio.Pages.Admin.User
{
    public class ManageLeaveRegisterRequest
    {
        public string LeaveType { get; set; }

        //public int LeaveTypeId { get; set; }
        public double TotalLeave { get; set; }

        public string UserId { get; set; }

        public DateTime ApprovedFromDate { get; set; }

        public DateTime ApprovedToDate { get; set; }
    }
}
