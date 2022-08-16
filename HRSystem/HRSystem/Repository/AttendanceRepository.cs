using HRSystem.Data;
using HRSystem.Models;

namespace HRSystem.Repository
{
	public class AttendanceRepository:IAttendanceRepository
    {
        ApplicationDbContext context;

        public AttendanceRepository(ApplicationDbContext db)
        { context = db; }

        public List<Attendance> GetAllRecords()
        {
            return context.Attendances.ToList();
        }


        //=============================================================//
        //================= Start Get Selected Record =================//

        public Attendance GetRecordByID(int id)
        {
            Attendance attendance = context.Attendances.FirstOrDefault(e => e.Id == id);
            return attendance;
        }

        //================== End Get Selected Record ==================//
        //=============================================================//



        //=============================================================//
        //================= Start Display All Records =================//

        // Filter All Records And Get Only Our Count Of Record To Display
        public List<Attendance> GetCountOfRecords(int countOfRecors, int rowNumber)
        {

            if (rowNumber == 1)
            { return context.Attendances.Take(countOfRecors).ToList(); }
            else
            { return context.Attendances.Skip((rowNumber - 1) * countOfRecors).Take(countOfRecors).ToList(); }

        }

        // Get Our Records In Model View To Display With Full Data
        public List<fullAttendanceData> GetAllRecordsInMV(int countOfRecors, int rowNumber)
        {
            List<Attendance> attendanceList = this.GetCountOfRecords(countOfRecors, rowNumber);
            List<Department> deptList = context.Departments.ToList();
            List<Employee> empList = context.Employees.ToList();

            List<fullAttendanceData> fullAttendanceData = new List<fullAttendanceData>();

            foreach (var attenItem in attendanceList)
            {
                fullAttendanceData atten = new fullAttendanceData();
                atten.Id = attenItem.Id;
                atten.AttendanceTime = attenItem.AttendanceTime;
                atten.AbsenceTime = attenItem.AbsenceTime;
                atten.Date = attenItem.Date;

                foreach (var deptItem in deptList)
                {
                    if (attenItem.DeptId == deptItem.Id)
                    { atten.DeptName = deptItem.Name; }
                }
                foreach (var empItem in empList)
                {
                    if (attenItem.EmpId == empItem.Id)
                    { atten.EmpName = empItem.Name; }
                }
                fullAttendanceData.Add(atten);
            }

            return fullAttendanceData;
        }
        
        public int GetCountOfAllRecords()
        { return GetAllRecords().Count; }

        //================== End Display All Records ==================//
        //=============================================================//



        // Get All Records To Filter By EmpName, DeptName Or Date
        public List<fullAttendanceData> GetAllRecordsMVForFilter()
        {
            List<Attendance> attendanceList = this.GetAllRecords();
            List<Department> deptList = context.Departments.ToList();
            List<Employee> empList = context.Employees.ToList();

            List<fullAttendanceData> fullAttendanceData = new List<fullAttendanceData>();

            foreach (var attenItem in attendanceList)
            {
                fullAttendanceData atten = new fullAttendanceData();
                atten.Id = attenItem.Id;
                atten.AttendanceTime = attenItem.AttendanceTime;
                atten.AbsenceTime = attenItem.AbsenceTime;
                atten.Date = attenItem.Date;

                foreach (var deptItem in deptList)
                {
                    if (attenItem.DeptId == deptItem.Id)
                    { atten.DeptName = deptItem.Name; }
                }
                foreach (var empItem in empList)
                {
                    if (attenItem.EmpId == empItem.Id)
                    { atten.EmpName = empItem.Name; }
                }
                fullAttendanceData.Add(atten);
            }

            return fullAttendanceData;
        }



        //============================================================//
        //================= Start Search By Emp Name =================//

        List<fullAttendanceData> GetListOfFilteredRecoredsByEmpName(string searchText, string searchBy)
        {
            List<fullAttendanceData> attend = this.GetAllRecordsMVForFilter();
            List<fullAttendanceData> finalFilteredData = new List<fullAttendanceData>();

            if (searchBy == "empName")
            {
                foreach (fullAttendanceData attendItem in attend)
                {
                    if (attendItem.EmpName.ToLower().Contains(searchText.ToLower()))
                    { finalFilteredData.Add(attendItem); }
                }
            }
            else if (searchBy == "deptName")
            {
                foreach (fullAttendanceData attendItem in attend)
                {
                    if (attendItem.DeptName.ToLower().Contains(searchText.ToLower()))
                    { finalFilteredData.Add(attendItem); }
                }
            }
            else if (searchText == "PrintAll" && searchBy == "PrintAll")
            {
                foreach (fullAttendanceData attendItem in attend)
                {
                    finalFilteredData.Add(attendItem);
                }
            }

            return finalFilteredData;
        }

        public List<fullAttendanceData> GetFilteredRecoredsByEmpName(string searchText, string searchBy, int countOfRecors, int rowNumber)
        {

            List<fullAttendanceData> finalFilteredData = GetListOfFilteredRecoredsByEmpName(searchText, searchBy);

            if (rowNumber == 1)
            { return finalFilteredData.Take(countOfRecors).ToList(); }
            else
            { return finalFilteredData.Skip((rowNumber - 1) * countOfRecors).Take(countOfRecors).ToList(); }

        }

        public int GetCountOfFilteredRecoredsByEmpName(string searchText, string searchBy)
		{ return GetListOfFilteredRecoredsByEmpName(searchText, searchBy).Count; }

        //================== End Search By Emp Name ==================//
        //============================================================//



        //============================================================//
        //=================== Start Search By Date ===================//

        List<fullAttendanceData> GetListOfFilteredRecoredsByDate(DateTime startDate, DateTime endDate) 
        {
            List<fullAttendanceData> attend = this.GetAllRecordsMVForFilter();

            if (startDate.ToString().Trim() != "1/1/0001 12:00:00 AM" && endDate.ToString().Trim() != "1/1/0001 12:00:00 AM")
            { attend = attend.Where(x => x.Date >= startDate && x.Date <= endDate).ToList(); }
            return attend;
        }

        public List<fullAttendanceData> GetFilteredRecoredsByDate(DateTime startDate, DateTime endDate, int countOfRecors, int rowNumber)
        {
            List<fullAttendanceData> attend = GetListOfFilteredRecoredsByDate(startDate, endDate);

            if (rowNumber == 1)
            { return attend.Take(countOfRecors).ToList(); }
            else
            { return attend.Skip((rowNumber - 1) * countOfRecors).Take(countOfRecors).ToList(); }
        }

        public int GetCountOfFilteredRecoredsByDate(DateTime startDate, DateTime endDate)
        {
            return GetListOfFilteredRecoredsByDate(startDate, endDate).Count;
        }
        
        //==================== End Search By Date ====================//
        //============================================================//



        //============================================================//
        //========== Start Insert, Update And Delete Record ==========//

        public void Insert(Attendance newAttendance)
        {
            Attendance checkFoundEmp = this.GetAllRecords().FirstOrDefault(e => e.EmpId == newAttendance.EmpId && e.Date == newAttendance.Date);

            if (checkFoundEmp == null)
            {
                context.Attendances.Add(newAttendance);
                context.SaveChanges();
            }
            else
            { throw new Exception("Employee already add to attendance"); }
        }

        public void Update(int id, Attendance newAttendance)
        {
            Attendance oldAttendance = GetRecordByID(id);

            oldAttendance.AttendanceTime = newAttendance.AttendanceTime;
            oldAttendance.AbsenceTime = newAttendance.AbsenceTime;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Attendance attend = GetRecordByID(id);
            context.Attendances.Remove(attend);
            context.SaveChanges();
        }

        //=========== End Insert, Update And Delete Record ===========//
        //============================================================//

    }
}
