using System.ComponentModel.DataAnnotations;
using myportfolio.Model.Entity.Shared;

namespace myportfolio.Model.Entity.Resources
{
    public class ProductCatalog : Base
    {
        public int FileGroupId { get; set; }
        public FileGroup FileGroup { get; set; }

        [Required]
        [StringLength(200)]
        public string FileName { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        [Required]
        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
