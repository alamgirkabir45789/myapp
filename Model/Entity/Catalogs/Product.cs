using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Catalogs
{
    public class Product:Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }

        [StringLength(200)]
        public string ProjectUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
