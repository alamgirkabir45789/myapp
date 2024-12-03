using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Styles
{
    public class Style : Base
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
        public StyleCategory StyleCategory { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
