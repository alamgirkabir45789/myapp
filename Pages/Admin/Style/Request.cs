using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.Style
{
    public class Request
    {
        [Required]
        [StringLength(100)]
        public string StyleNo { get; set; }

        [StringLength(1000)]
        public string Fabrication { get; set; }

        public double Price { get; set; }

        [StringLength(10)]
        public string PriceFor { get; set; }

        [StringLength(100)]
        public string ExportTo { get; set; }

        [StringLength(500)]
        public string Size { get; set; }

        [StringLength(500)]
        public string Color { get; set; }

        [StringLength(250)]
        public string Item { get; set; }

        public int StyleCategoryId { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public List<Model.Entity.Styles.StyleCategory> Categories { get; set; }
        public Model.Entity.Styles.Style Style { get; set; }
    }
}
