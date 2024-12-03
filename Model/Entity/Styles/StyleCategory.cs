using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Styles
{
    public class StyleCategory : Base
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public ICollection<Style> Styles { get; set; }
    }
}
