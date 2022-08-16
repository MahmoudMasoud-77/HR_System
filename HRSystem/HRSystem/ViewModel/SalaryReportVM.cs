using HRSystem.Models;

namespace HRSystem.ViewModels
{
    public class SalaryReportVM
    {
        //public List<Employee> employees;
        //public List<Attendance> empAttendances;
        //public GeneralSettings generalSettings;
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Department { get; set; }
        public int AttendanceDays { get; set; }
        public int AbsenceDays { get; set; }
        public double ExtraTime { get; set; }
        public int Deduction { get; set; }
        public double TotalExtraTime { get; set; }
        public int TotalDeduction { get; set; }
        public double FinalSalary { get; set; }
    }
}
