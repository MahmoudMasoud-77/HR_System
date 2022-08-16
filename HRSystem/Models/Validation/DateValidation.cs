using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models.Validation
{
    public class DateValidation: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime Date = DateTime.Parse(value.ToString());

                if (Date.Year <= 0001 )
                {
                    return new ValidationResult("Date Is Required");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Date Is Required");        
        }
    }
}
