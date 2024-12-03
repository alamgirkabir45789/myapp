using System.ComponentModel.DataAnnotations;

namespace myportfolio.Pages.Admin.User
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EmpCode { get; set; }
        public string Role { get; set; }

        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
      

    }
}
