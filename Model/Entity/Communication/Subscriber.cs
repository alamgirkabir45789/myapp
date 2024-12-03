using myportfolio.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace myportfolio.Model.Entity.Communication
{
    public class Subscriber:Base
    {
       
        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string IpAddress { get; set; }

        [DefaultValue(false)]
        public bool IsResponse { get; set; }
    }
}
