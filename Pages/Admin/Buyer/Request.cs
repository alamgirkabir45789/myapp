using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.Buyer
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [Required]
        [StringLength(500)]
        public string FileName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } 
        
        [StringLength(1000)]
        public string Profession { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
