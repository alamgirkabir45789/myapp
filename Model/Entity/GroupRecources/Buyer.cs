using myportfolio.Model.Entity.Catalogs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.GroupRecources
{
    public class Buyer : Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Profession { get; set; }

        [Required]
        [StringLength(250)]
        public string LogoPath { get; set; }
    }
}
