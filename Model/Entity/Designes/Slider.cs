using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Designes
{
    public class Slider:Base
    {
        [Required]
        [StringLength(200)]
        public string Identifier { get; set; }

        [StringLength(200)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
