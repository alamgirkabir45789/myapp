using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.AboutUs
{
    public class Compliance:Base
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(10000)]
        public string Details { get; set; }

        [Required]
        [StringLength(250)]
        public string ImagePath { get; set; }
        
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
