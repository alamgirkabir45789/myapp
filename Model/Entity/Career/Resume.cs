using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Career
{
    public class Resume : Base
    {
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Phone { get; set; }

        public int YearsOfExperience { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }

        public JobCircular JobCircular { get; set; }
        public int? JobCircularId { get; set; }
    }
}
