using myportfolio.Model.Entity.Catalogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.DailyReport
{
    public class DailyReport : Base
    {

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(200)]
        //public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string ProjectName { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AssignDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TargetDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DefaultValue(false)]
        public bool IsCompleted { get; set; }

        public ICollection<Model.Entity.DailyTask.DailyTask> DailyTasks { get; set; }
    }
}
