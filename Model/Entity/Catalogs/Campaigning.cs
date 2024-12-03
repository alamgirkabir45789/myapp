using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Catalogs
{
    public class Campaigning : Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }

        [Required]
        [StringLength(200)]
        public string BusinessType { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
