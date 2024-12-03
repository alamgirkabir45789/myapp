using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Communication
{
    public class Query:Base
    {
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string PhoneNo { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        [DefaultValue(false)]
        public bool IsRead { get; set; }
    }
}
