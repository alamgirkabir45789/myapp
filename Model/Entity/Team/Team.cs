using myportfolio.Model.Entity.GroupRecources;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Team
{
    public class Team:Base
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]      
        public int SortCode { get; set; }

       
        public string UserId { get; set; }


        [EmailAddress]
        public string Email { get; set; }

     
        public string ContactNo { get; set; }

        [StringLength(300)]
        public string Expertise { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }


        [DefaultValue(true)]
        public bool isActive { get; set; }

        [Required]
        [StringLength(250)]
        public string LogoPath { get; set; }
    }
}
