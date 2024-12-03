using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.ViewModels
{
    public class HolidayRequest
    {

        public int Id { get; set; }

        public Guid Key { get; set; }

        public string HEvents { get; set; }


        [StringLength(200)]
        public string HDescription { get; set; }

        public DateTime HEventDate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
