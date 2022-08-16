using Microsoft.AspNetCore.Authorization;
namespace HRSystem.Filter
{
    public class PermissionRequirment:IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirment(string permission)
        {
            Permission = permission;
        }
    }
}
