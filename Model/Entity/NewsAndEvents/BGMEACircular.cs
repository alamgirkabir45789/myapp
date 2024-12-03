using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.NewsAndEvents
{
    public class BGMEACircular : Base
    {
        [Required]
        [StringLength(500)]
        public string CircularTitle { get; set; }

        [StringLength(10000)]
        public string CircularDetails { get; set; }

        [Required]
        [StringLength(100)]
        public string Language { get; set; }

        public DateTime PublishDate { get; set; }
        
        [StringLength(50)]
        public string FileType { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
