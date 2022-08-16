using HRSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.ViewModels
{
    public class EmployeeVM
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$",ErrorMessage = "Email must be properly formatted.")]
        public string? Email { get; set; }
        [RegularExpression("[0-9]{14}", ErrorMessage = "Invalid National ID ,Must be 14 Digit")]
        [Display(Name = "National Id")]
        public string NationalId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Range(5000, 10000, ErrorMessage = "Salary Must Be Between (5K,10K) ")]
        public double Salary { get; set; }
        public string Nationality { get; set; }
        [RegularExpression("01[0125]{1}[0-9]{8}", ErrorMessage = "Invalid Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        public TGender Gender { get; set; }

        [Required]
        [DataType(DataType.Time)]

        public TimeSpan Start { get; set; }
        [Required]
        [DataType(DataType.Time)]

        public TimeSpan End { get; set; }
        [Required]
        [DataType(DataType.Date)]

        public DateTime ContractDate { get; set; }

        [Required]
        public int DeptId { get; set; }
    }
}
