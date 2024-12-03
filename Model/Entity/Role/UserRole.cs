using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.Model.Entity.Role
{
    public class UserRole:IdentityRole
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
