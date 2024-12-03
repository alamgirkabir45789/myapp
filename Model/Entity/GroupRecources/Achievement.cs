using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.GroupRecources
{
    public class Achievement:Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public DateTime Achieved { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
