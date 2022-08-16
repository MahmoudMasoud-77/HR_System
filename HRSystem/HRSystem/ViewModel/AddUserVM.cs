using System.ComponentModel.DataAnnotations;

namespace HRSystem.ViewModel
{
    public class AddUserVM
    {
        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2}", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        //[StringLength(100,ErrorMessage ="The {0} must be at least {2}",MinimumLength =3)]
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The password and confirm Password is not match")]
        public string ConfirmPassword { get; set; }

        public List<CheckboxVM> Roles { get; set; }
    }
}
