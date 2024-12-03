using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace myportfolio.Pages.Admin.Team
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(500)]
        public string FileName { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int SortCode { get; set; }

        public string UserId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

     
        public string ContactNo { get; set; }

        [StringLength(1000)]
        public string Designation { get; set; }

        [StringLength(300)]
        public string Expertise { get; set; }

        [DefaultValue(true)]
        public bool isActive { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
