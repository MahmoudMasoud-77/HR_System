using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Filter
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirment>
    {
        public PermissionAuthorizationHandler()
        {

        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
        {
            if (context.User == null)
            {
                return;
            }
            var canAccess = context.User.Claims.Any(x=>x.Type== "Permission" && x.Value==requirement.Permission && x.Issuer=="LOCAL AUTHORITY");
            if (canAccess)
            {
                context.Succeed(requirement);
                return;
            }
        
        }
    }
}
