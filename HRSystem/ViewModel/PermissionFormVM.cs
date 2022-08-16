namespace HRSystem.ViewModel
{
    public class PermissionFormVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<CheckboxVM> RoleClaims { get; set; }
    }
}
