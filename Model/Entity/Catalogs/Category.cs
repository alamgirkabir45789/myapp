using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Catalogs
{
    public class Category:Base
    {
       
        public string BusinessType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
