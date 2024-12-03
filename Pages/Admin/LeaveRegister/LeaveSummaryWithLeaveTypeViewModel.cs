using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.Pages.Admin.LeaveRegister
{
    public class LeaveSummaryWithLeaveTypeViewModel
    {
        public string LeaveType { get; set; }
        public double TotalLeave { get; set; }
        public double RemainingLeave { get; set; }
        public double TakenLeave { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidToDate { get; set; }
    }
}
