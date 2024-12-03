using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace myportfolio.ViewModels
{
    public class ResetPasswordViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirm password must be matched")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
