using HRSystem.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.Models
{
    public class Employee
    {
        //public Employee()
        //{
        //    Attendances = new List<Attendance>();
        //}
        

        //Personal Data
        public int Id { get; set; }
        public string Name { get; set; }
        //[UniqueEmail]
        //public string? Email { get; set; }
        public TGender Gender { get; set; }
        public string Address { get; set; }
        [RegularExpression("01[0125]{1}[0-9]{8}", ErrorMessage = "Phone Must be 11 Numbers Begins with (010,011,012,015)")]
        //[RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number Must be 11 Numbers.")]
        [Phone(ErrorMessage = "Invalid Mobile Number.")]
        //[UniquePhone]
        public string Phone { get; set; }
        public string Nationality { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        [BirthDateValidation(20)]
        [Display(Name = "Date of Birth")]
        [DateValidation]
        [Required]
        public DateTime BirthDay { get; set; }
        [Display(Name = "National Id")]
        [RegularExpression(@"^([0-9]{14})$", ErrorMessage = "Invalid National_Id Must be 14 Numbers.")]
        public string NationalId { get; set; }

        //Work Data
        [DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:C0}")]
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(5000, 10000, ErrorMessage = "Salary Must Be Between (5K,10K) ")]
        public double Salary { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime AttendanceTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime AbsenceTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        [HireDateValidation(2000,1,1)]
        [Required]
        [DateValidation]
        public DateTime HireDate { get; set; }

        //Relations
        public virtual List<Attendance>? Attendances { get; set; }

        [ForeignKey("Department")]
        //[Required(ErrorMessage = "Department is Required")]
        public int? DeptId { get; set; }
        public virtual Department? Department { get; set; }
    }

    public enum TGender
    {
        [Display(Name = "Male")]
        Male=1,
        [Display(Name = "Female")]
        Female=2
    }
}
