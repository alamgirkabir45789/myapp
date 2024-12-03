using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Designes
{
    public class HeadLine : Base
    {
        [Required]
        [StringLength(500)]
        public string News { get; set; }
        public bool IsActive { get; set; }
    }
}
