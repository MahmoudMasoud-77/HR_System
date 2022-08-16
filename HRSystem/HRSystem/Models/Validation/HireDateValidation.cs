using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models.Validation
{
    public class HireDateValidation : ValidationAttribute
    {
        private int startCompanyOfYear;
        private int startCompanyOfMonth;
        private int startCompanyOfDay;

        public HireDateValidation(int _startCompanyOfYear ,int _startCompanyOfMonth,int _startCompanyOfDay)
        {
            this.startCompanyOfYear = _startCompanyOfYear;
            this.startCompanyOfMonth = _startCompanyOfMonth;
            this.startCompanyOfDay = _startCompanyOfDay;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string Massage = "HireDate Is Invalid Must be after " + startCompanyOfDay + "/" +
                                  startCompanyOfMonth + "/" + startCompanyOfYear;
                DateTime hiredate = DateTime.Parse(value.ToString());
                if (hiredate.Year > startCompanyOfYear  && hiredate.Day > startCompanyOfDay)
                    return ValidationResult.Success;
                else if(hiredate.Year == startCompanyOfYear)
                {
                    if(hiredate.Month > startCompanyOfMonth)
                        return ValidationResult.Success;
                    else if(hiredate.Month == startCompanyOfMonth)
                    {
                        if(hiredate.Day >= startCompanyOfDay)
                            return ValidationResult.Success;
                        else 
                            return new ValidationResult(Massage);
                    }
                    else
                        return new ValidationResult(Massage);
                }

                else
                    return new ValidationResult(Massage);
            }
            return new ValidationResult("HireDate Is Required");
        }
    }
}
