using HRSystem.Data;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models.Validation
{
    public class UniqueEmail: ValidationAttribute
    {
        ApplicationDbContext context = new ApplicationDbContext();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                //string Email = value.ToString();
                //Employee Emp = context.Employees.FirstOrDefault(c => c.Email == Email);
                //if (Emp == null)
                //    return ValidationResult.Success;
                return new ValidationResult("This email already exists");
            }
            return new ValidationResult("Email Requird");
        }
    }
}
