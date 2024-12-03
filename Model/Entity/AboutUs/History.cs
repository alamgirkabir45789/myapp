using myportfolio.Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.AboutUs
{
    public class History:Base
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
