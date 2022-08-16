using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRSystem.Repository;


namespace HRSystem.Models
{
    public class Attendance
    {

        public int Id { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{HH:mm:ss}")]
        [Remote("CheckAbsenceTimeWithAttendanceTime", "Attendance",
            AdditionalFields = "AbsenceTime"
            , ErrorMessage = "The Attendance Time must be more than The Absence Time")]
        public DateTime AttendanceTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{HH:mm:ss}")]
        [Remote("CheckAbsenceTimeWithAttendanceTime", "Attendance",
            AdditionalFields = "AttendanceTime"
            , ErrorMessage = "The Absence Time must be more than The Attendance Time")]
        public DateTime AbsenceTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{DD/MM/YYYY}")]
        public DateTime Date { get; set; }

        //Relations
        [ForeignKey("Employee")]
        public int EmpId { get; set;}
        public virtual Employee? Employee { get; set; }

        [ForeignKey("Department")]
        public int DeptId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
