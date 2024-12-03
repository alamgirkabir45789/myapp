using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.MediaGallery
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
        public string MediaType { get; set; }

        public DateTime EventDate { get; set; }


        [StringLength(250)]
        public string VideoLink { get; set; }



        public IFormFile[] File { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
