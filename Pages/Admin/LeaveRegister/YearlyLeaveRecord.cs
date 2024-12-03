namespace myportfolio.Pages.Admin.LeaveRegister
{
    public class YearlyLeaveRecord
    {
        public int TotalCasualLeave { get; set; }
        public int TotalSickLeave { get; set; }
        public int TotalEarnLeave { get; set; }
        public int TotalAnualLeave { get; set; }
        public int TotalHalfDayLeave { get; set; }
        public int TotalShortLeave { get; set; }
        public int TotalMaternityLeave { get; set; }

        public int RemainingCasualLeave { get; set; }
        public int RemainingSickLeave { get; set; }
        public int RemainingEarnLeave { get; set; }
        public int RemainingAnualLeave { get; set; }
        public double RemainingHalfDayLeave { get; set; }
        public int RemainingShortLeave { get; set; }
        public int RemainingMaternityLeave { get; set; }

        public int CasualLeave { get; set; }
        public int SickLeave { get; set; }
        public int EarnLeave { get; set; }
        public int AnualLeave { get; set; } 
        public double HalfDayLeave { get; set; } 
        public int ShortLeave { get; set; } 
        public int MaternityLeave { get; set; } 
    }
}
