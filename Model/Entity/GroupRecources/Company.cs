using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.GroupRecources
{
    public class Company:Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(10000)]
        public string Description { get; set; }

        [StringLength(10000)]
        public string History { get; set; }

        public DateTime Established { get; set; }

        [Required]
        [StringLength(250)]
        public string LogoPath { get; set; }
    }
}
