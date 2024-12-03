using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Shared
{
    public class FileGroup : Base
    {
        [Required]
        [StringLength(100)]
        public string GroupFor { get; set; }

        [Required]
        [StringLength(250)]
        public string GroupName { get; set; }
    }
}
