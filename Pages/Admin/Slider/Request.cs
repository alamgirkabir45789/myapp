using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.Slider
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(200)]
        public string Identifier { get; set; }

        [StringLength(200)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
