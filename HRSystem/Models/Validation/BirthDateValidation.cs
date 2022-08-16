using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models.Validation
{
    public class BirthDateValidation : ValidationAttribute
    {
        private int _Limit;
        public BirthDateValidation(int Limit)
        { // The constructor which we use in modal.
            this._Limit = Limit;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if(value != null)
            {
                DateTime bday = DateTime.Parse(value.ToString());
                DateTime today = DateTime.Today;
                int age = today.Year - bday.Year;

                // Go back to the year in which the person was born in case of a leap year
                if (bday > today.AddYears(-age))
                {
                    age--;
                }
                if (age < _Limit)
                {
                    return new ValidationResult("should Age greater than 20 old");
                }


                return ValidationResult.Success;
            }
            return new ValidationResult("BirthDate Is Required");

        }
    }
}
