using System.ComponentModel.DataAnnotations;

namespace myportfolio.ViewModels
{
    public class Signup
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactNo { get; set; }
        public string EmployeeCode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password did not match")]
        public string ConfirmPassword { get; set; }
    }
}
