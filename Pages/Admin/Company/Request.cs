using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.Company
{
    public class Request
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        [StringLength(500)]
        public string Name { get; set; }
        public string Title { get; set; }

        [StringLength(10000)]
        public string Description { get; set; }
        
        [StringLength(10000)]
        public string History { get; set; }

        public DateTime Established { get; set; }

        public int SortOrder { get; set; }

        public int CompanyId { get; set; }

        public IFormFile File { get; set; }

        public string LogoPath { get; set; }
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
