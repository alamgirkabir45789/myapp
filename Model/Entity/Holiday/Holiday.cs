using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Holiday
{
    public class Holiday:Base
    {
        [Required]
        [StringLength(200)]
        public string HEvents { get; set; }

        
        [StringLength(200)]
        public string HDescription { get; set; }

        public DateTime HEventDate { get; set; }
    }
}
