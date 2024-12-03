using System.Collections.Generic;

namespace myportfolio.Helpers
{
    public class LeaveTypeAndValue
    {
        public int Id { get; set; }
        public int CASUAL { get; set; }
        public int EARN { get; set; }
        public int SICK { get; set; }
        public double HALFDAY { get; set; }
        public int SHORTTERM { get; set; }
        public int ANNUAL { get; set; }
        public int MATERNITY { get; set; }

        public LeaveTypeAndValue()
        {

            EARN = 18;
            CASUAL = 10;
            SICK = 14;
            HALFDAY = 0.5;
            SHORTTERM = 0;
            ANNUAL = 20;
            MATERNITY = 18;
        }
       
    }

    
}
