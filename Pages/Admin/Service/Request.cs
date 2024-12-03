using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;

namespace myportfolio.Pages.Admin.Service
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        public string Description { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int CategoryId { get; set; }
        public string BusinessType { get; set; }
    }
}
