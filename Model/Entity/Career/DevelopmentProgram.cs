﻿using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Career
{
    public class DevelopmentProgram : Base
    {
        [Required]
        [StringLength(500)]
        public string FileName { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}