using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Resources
{
    public class MediaGallery : Base
    {
        [Required]
        [StringLength(200)]
        public string FileName { get; set; }

        [Required]
        [StringLength(200)]
        public string MediaType { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [StringLength(250)]
        public string VideoLink { get; set; }        
      

        [StringLength(50)]
        public string FileType { get; set; }



        [StringLength(250)]
        public string FilePath { get; set; }
    }
}
