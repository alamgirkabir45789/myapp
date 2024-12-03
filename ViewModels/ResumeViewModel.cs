using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace myportfolio.ViewModels
{
    public class ResumeViewModel
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

        public int? JobCircularId { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string Message { get; set; }
    }
}
