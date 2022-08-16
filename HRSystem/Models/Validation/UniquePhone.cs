using HRSystem.Data;
using System.ComponentModel.DataAnnotations;
namespace HRSystem.Models.Validation
{
    public class UniquePhone: ValidationAttribute
    {
        ApplicationDbContext context = new ApplicationDbContext();
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string Phone = value.ToString();
                Employee Emp = context.Employees.FirstOrDefault(c => c.Phone == Phone);
                if (Emp == null)
                    return ValidationResult.Success;
                return new ValidationResult("This Phone already exists");
            }
            return new ValidationResult("Phone Requird");
        }
    }
}
