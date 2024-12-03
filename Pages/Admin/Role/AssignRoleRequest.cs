namespace myportfolio.Pages.Admin.Role
{
    public class AssignRoleRequest
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
