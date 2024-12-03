using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace myportfolio.Model.Entity.ReportedProject
{
    public class ReportedProject:Base
    {
      

        [Required]
        [StringLength(300)]
        public string ProjectName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; }
    }
}
