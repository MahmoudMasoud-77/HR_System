using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
	public class fullAttendanceData
	{
		public int Id { get; set; }

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
		public DateTime Date { get; set; }
		public string EmpName { get; set; }
		public string DeptName { get; set; }
    }
}
