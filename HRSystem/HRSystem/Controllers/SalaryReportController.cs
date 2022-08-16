using HRSystem.Constants;
using HRSystem.Data;
using HRSystem.Models;
using HRSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HRSystem.Controllers
{
    public class SalaryReportController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        static DateTime searchMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
        List<SalaryReportVM> salaryReport;
        List<Employee> employees;
        GeneralSettings generalSettings;
        public SalaryReportController()
        {
            //context = new HREntity();
        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
        [Authorize(Permissions.Holidays.View)]
        public IActionResult Index()
        {
            
            salaryReport = new List<SalaryReportVM>();
            employees = context.Employees.Include(e => e.Attendances).ToList();
            generalSettings = context.GeneralSettings.FirstOrDefault();

            ViewData["MonthList"] = new SelectList(Enumerable.Range(1, 12).Select(x =>
                new SelectListItem()
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1], //+ " (" + x + ")",
                    Value = x.ToString()
                }), "Value", "Text");

            ViewData["YearsList"] = new SelectList(Enumerable.Range(2000, DateTime.Now.Year - 2000 + 1).Select(x =>
               new SelectListItem()
               {
                   Text = x.ToString(),
                   Value = x.ToString()
               }), "Value", "Text");

            ViewData["CurrentDate"] = searchMonth;

            return View(GetSalaryForMonth(employees, salaryReport, searchMonth));
        }

        private List<SalaryReportVM> GetSalaryForMonth(List<Employee> employees, List<SalaryReportVM> salaryReport, DateTime dateTime)
        {
            foreach (Employee emp in employees)
            {
                SalaryReportVM currentEmpData = new SalaryReportVM();
                currentEmpData.Name = emp.Name;
                currentEmpData.Salary = emp.Salary;
                var attendanceList = emp.Attendances.Where(d => d.Date.Month == dateTime.Month && d.Date.Year == dateTime.Year).ToList();
                currentEmpData.Department = context.Departments.Where(d => d.Id == emp.DeptId).Select(d => d.Name).FirstOrDefault();
                currentEmpData.AttendanceDays = attendanceList.Count();
                currentEmpData.AbsenceDays = DateTime.DaysInMonth(dateTime.Year, dateTime.Month) - currentEmpData.AttendanceDays - NumOfWeekends(dateTime) - GetHolidays(dateTime);
                currentEmpData.ExtraTime = CalcExtraTime(attendanceList, emp);
                currentEmpData.Deduction = CalcDeduction(attendanceList, emp);//, currentEmpData.AbsenceDays);
                currentEmpData.TotalExtraTime = currentEmpData.ExtraTime * generalSettings.ExtraTime;
                currentEmpData.TotalDeduction = (currentEmpData.Deduction + (emp.AbsenceTime.Hour - emp.AttendanceTime.Hour) * currentEmpData.AbsenceDays) * generalSettings.Deduction;
                double empHourlyRate = EmpHourlyRate(emp);
                currentEmpData.FinalSalary = (int)(emp.Salary + (currentEmpData.TotalExtraTime * empHourlyRate) - (currentEmpData.TotalDeduction * empHourlyRate));

                salaryReport.Add(currentEmpData);
            }
            return salaryReport;
        }

        private int GetHolidays(DateTime thisMonth)
        {
            int holidaysThisMonth = 0;
            var holidaysData = context.Holidays.ToList();
            foreach (var holidayData in holidaysData)
                if (thisMonth.Month == holidayData.Date.Month)
                    holidaysThisMonth += 1;
            return holidaysThisMonth;
        }

        public IActionResult ChangeMonth(int month, int year)
        {
            searchMonth = new DateTime(year, month, 1);
            return RedirectToAction("Index");
        }
        private double EmpHourlyRate(Employee emp)
        {
            int daysInMonth = DateTime.DaysInMonth(searchMonth.Year, searchMonth.Month);
            int weekends = NumOfWeekends(searchMonth);
            int numOfWorkDays = daysInMonth - weekends;
            double hourlyRate = emp.Salary / (numOfWorkDays * (emp.AbsenceTime.Hour - emp.AttendanceTime.Hour));
            return hourlyRate;
        }

        private int CalcDeduction(List<Attendance> attendances, Employee emp)//, int absenceDays)
        {
            int deduction = 0;
            foreach (var attendance in attendances)
            {
                if (attendance.AttendanceTime > emp.AttendanceTime)
                    deduction += attendance.AttendanceTime.Hour - emp.AttendanceTime.Hour;
                if (attendance.AbsenceTime < emp.AttendanceTime)
                    deduction += emp.AttendanceTime.Hour - attendance.AttendanceTime.Hour;
            }
            //deduction += (emp.AbsenceTime - emp.AttendanceTime) * absenceDays;
            return deduction;
        }

        private double CalcExtraTime(List<Attendance> attendances, Employee emp)
        {
            double extraTime = 0;
            foreach (var attendance in attendances)
            {
                if (attendance.AttendanceTime < emp.AttendanceTime)
                    extraTime += emp.AttendanceTime.Hour - attendance.AttendanceTime.Hour;
                if (attendance.AbsenceTime > emp.AbsenceTime)
                    extraTime += attendance.AbsenceTime.Hour - emp.AbsenceTime.Hour;
            }
            return extraTime;
        }

        private int NumOfWeekends(DateTime thisMonth)
        {
            var numOfWeekends = 0;
            int year = thisMonth.Year;
            int month = thisMonth.Month;
            int daysThisMonth = DateTime.DaysInMonth(year, month);
            DateTime firstOfMonth = new DateTime(year, month, 1);
            if (generalSettings.Weekend1st >= DayOfWeek.Sunday && generalSettings.Weekend1st <= DayOfWeek.Saturday)//== DayOfWeek.Friday)
            {
                if (generalSettings.Weekend2nd >= DayOfWeek.Sunday && generalSettings.Weekend2nd <= DayOfWeek.Saturday)
                {
                    for (int i = 1; i <= daysThisMonth; i++)
                    {
                        if (firstOfMonth.DayOfWeek == generalSettings.Weekend1st || firstOfMonth.DayOfWeek == generalSettings.Weekend2nd)
                            numOfWeekends++;
                        firstOfMonth = firstOfMonth.AddDays(1);
                    }
                    return numOfWeekends;
                }
                for (int i = 1; i <= daysThisMonth; i++)
                {
                    if (firstOfMonth.DayOfWeek == DayOfWeek.Friday)
                        numOfWeekends++;
                    firstOfMonth = firstOfMonth.AddDays(1);
                }
                return numOfWeekends;
            }
            return numOfWeekends;
        }
    }
}