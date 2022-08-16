using HRSystem.Constants;
using HRSystem.Models;
using HRSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRSystem.Data;
namespace HRSystem.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.context = context;
        }
        [Authorize(Permissions.Users.View)]
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = context.Users.ToList();
            List<UserVM> usersViewModel = new List<UserVM>(); 
            foreach (var user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    UserName=user.UserName,
                    Name=user.Name,
                    Email=user.Email,
                    Password=user.PasswordHash,
                    Role= _userManager.GetRolesAsync(user).Result
                };
                usersViewModel.Add(userVM);
            }


            return View(usersViewModel);
        }

        [Authorize(Permissions.Users.Create)]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new AddUserVM
            {
                Roles = roles.Select(role=>new CheckboxVM
                {
                    DisplayID = role.Id,
                    DisplayValue = role.Name,
                    IsSelected = false
                }).ToList()


            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserVM model)
        {
            //if (!ModelState.IsValid)
            //{
                
            //    return View(model);
            //}

            if (!model.Roles.Any(x => x.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please Select at least one role");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already exists");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "This User Name is already exists");
                return View(model);
            }
            
            var user = new ApplicationUser {
                UserName = model.UserName,
                Name = model.Name,
                Email=model.Email,
                PasswordHash=model.Password,

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("ConfirmPassword", error.Description);
                }
                return View(model);
            }
           
            await _userManager.AddToRolesAsync(user,model.Roles.Where(x=>x.IsSelected).Select(x=>x.DisplayValue));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRoleVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new CheckboxVM
                {
                    DisplayID = role.Id,
                    DisplayValue = role.Name,
                    IsSelected=_userManager.IsInRoleAsync(user,role.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles= await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(x => x == role.DisplayValue)&& !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.DisplayValue);
                }
                if (!userRoles.Any(x => x == role.DisplayValue) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.DisplayValue);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        
    }
}
