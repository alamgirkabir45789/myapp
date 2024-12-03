using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.GroupRecources
{
    public class CompanyImage : Base
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public int SortOrder { get; set; }

        [Required]
        [StringLength(250)]
        public string ImagePath { get; set; }
    }
}
