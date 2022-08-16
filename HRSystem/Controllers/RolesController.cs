using HRSystem.Constants;
using HRSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRSystem.Controllers
{
    
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager=roleManager;
        }
        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Roles.Create)]
        public async Task<IActionResult> Add(RoleFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            var roleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (roleExist)
            {
                ModelState.AddModelError("Name", "Role Is Exist");
                return View("Index", roleExist);
            }
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.generateAllPermissions();
            var allPermissions = allClaims.Select(x => new CheckboxVM { DisplayValue = x }).ToList();
            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(x => x == permission.DisplayValue))
                {
                    permission.IsSelected = true;
                }
                
            }
            var viewModel = new PermissionFormVM
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleClaims = allPermissions

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionFormVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }
            var roleClaims =await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var selectedClaims = model.RoleClaims.Where(x => x.IsSelected).ToList();

            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));
            }


            return RedirectToAction(nameof(Index));
        }





    }
}
