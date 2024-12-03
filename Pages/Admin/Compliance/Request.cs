using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.Compliance
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
