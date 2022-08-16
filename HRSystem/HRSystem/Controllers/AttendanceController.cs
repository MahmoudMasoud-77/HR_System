using HRSystem.Models;
using Microsoft.AspNetCore.Mvc;
using HRSystem.Repository;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using HRSystem.Constants;

namespace HRSystem.Controllers
{
	public class AttendanceController : Controller
    {

        IAttendanceRepository AttendanceRepo;
        IDepartment DepartmentRepo;
        IEmployee EmployeeRepo;

        public AttendanceController(IAttendanceRepository AttendanceRepo, IDepartment deptRepo, IEmployee employeeRepo)
		{
			this.AttendanceRepo = AttendanceRepo;
			DepartmentRepo = deptRepo;
			EmployeeRepo = employeeRepo;
		}


        //=============================================================//
        //================= Start Display All Records =================//
        [Authorize(Permissions.Attendance.View)]
        public IActionResult Index(int countOfRecors, int rowNumber)
        {
			List<fullAttendanceData> fullAttendanceData = AttendanceRepo.GetAllRecordsInMV(countOfRecors, 1);

            return View(fullAttendanceData);
        }

        public IActionResult GetAllDataByJson(int countOfRecors, int rowNumber)
        {
            List<fullAttendanceData> fullAttendanceData = AttendanceRepo.GetAllRecordsInMV(countOfRecors, rowNumber);
            return Json(fullAttendanceData); 
        }

        public IActionResult GetCountOfAllRecords()
        {
            return Json(AttendanceRepo.GetCountOfAllRecords());
        }

        //================== End Display All Records ==================//
        //=============================================================//



        //============================================================//
        //================= Start Search By Emp Name =================//

        public IActionResult GetCountOfFilteredRecordsByEmpName(string searchText, string searchBy)
        {
            return Json(AttendanceRepo.GetCountOfFilteredRecoredsByEmpName(searchText, searchBy));
        }

        public IActionResult FilterRecordsByEmpName(string searchText, string searchBy, int countOfRecors, int rowNumber)
        {
            List<fullAttendanceData> fillterdAttend = AttendanceRepo.GetFilteredRecoredsByEmpName(searchText, searchBy, countOfRecors, rowNumber);

            return Json(fillterdAttend);
        }

        //================== End Search By Emp Name ==================//
        //============================================================//



        //============================================================//
        //=================== Start Search By Date ===================//

        public IActionResult GetCountOfFilteredRecordsByDate(DateTime startDate, DateTime endDate)
        {
            return Json(AttendanceRepo.GetCountOfFilteredRecoredsByDate(startDate, endDate));
        }

        public ActionResult FilterRecordsByDate(DateTime startDate, DateTime endDate, int countOfRecors, int rowNumber)
        {
            List<fullAttendanceData> fillterdAttend = AttendanceRepo.GetFilteredRecoredsByDate(startDate, endDate, countOfRecors, rowNumber);

            return Json(fillterdAttend);
        }

        //==================== End Search By Date ====================//
        //============================================================//



        //============================================================//
        //==================== Start Edit Record =====================//
        [Authorize(Permissions.Attendance.Edit)]
        public IActionResult Edit(int id)
        {
            Attendance attendModel = AttendanceRepo.GetRecordByID(id);

            List<Department> deptList = DepartmentRepo.GetAll();
            List<Employee> empList = EmployeeRepo.GetAll();

            fullAttendanceData atten = new fullAttendanceData();
            atten.Id = attendModel.Id;

            atten.AttendanceTime = attendModel.AttendanceTime;
            atten.AbsenceTime = attendModel.AbsenceTime;
            atten.Date = attendModel.Date;

            foreach (var deptItem in deptList)
            {
                if (attendModel.DeptId == deptItem.Id)
                { atten.DeptName = deptItem.Name; }
            }
            foreach (var empItem in empList)
            {
                if (attendModel.EmpId == empItem.Id)
                { atten.EmpName = empItem.Name; }
            }
            
            return View(atten);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(int Id, Attendance newAttendance)
        {

            Attendance oldAttendance = AttendanceRepo.GetRecordByID(Id);
            if (oldAttendance != null)
            {
                AttendanceRepo.Update(Id, newAttendance);
                return RedirectToAction("Index");
            }
            
            return View("Edit", newAttendance);
        }

        //====================== End Edit Record =====================//
        //============================================================//



        //============================================================//
        //===================== Start Add Record =====================//
        [Authorize(Permissions.Attendance.Create)]
        [HttpGet]
        public IActionResult New()
        {
            ViewData["deptList"] = DepartmentRepo.GetAll();
            ViewData["empList"] = EmployeeRepo.GetAll();
            return View(new Attendance());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveNew(Attendance attend)
        {			
            if (ModelState.IsValid)
            {
                try
                {
                    AttendanceRepo.Insert(attend);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                { ModelState.AddModelError("EmpId", ex.Message.ToString()); }
            }
            ViewData["deptList"] = DepartmentRepo.GetAll();
            ViewData["empList"] = EmployeeRepo.GetAll();
            return View("New", attend);
        }

        //====================== End Add Record ======================//
        //============================================================//



        //============================================================//
        //================= Start Upload Excel Sheet =================//

        [HttpGet]
        public IActionResult GetFileExcel(List<Attendance> attendList = null)
        {
            attendList = attendList == null ? new List<Attendance>() : attendList;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GetFileExcel(IFormFile file, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
			try
			{
                string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
                using (FileStream fileStream = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
                var attendance = this.GetAttendanceListForExcel(file.FileName);
                foreach (Attendance attend in attendance)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        { AttendanceRepo.Insert(attend); }
                        catch (Exception ex)
                        { continue; }
                    }
                }
            }
			catch (Exception) { }


            return RedirectToAction("Index");
        }

        private List<Attendance> GetAttendanceListForExcel(string fName)
        {
            List<Attendance> attendance = new List<Attendance>();
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        attendance.Add(new Attendance()
                        {
                            AttendanceTime = Convert.ToDateTime(reader.GetValue(0)),
                            AbsenceTime = Convert.ToDateTime(reader.GetValue(1)),
                            Date = Convert.ToDateTime(reader.GetValue(2)),
                            EmpId = Convert.ToInt32(reader.GetValue(3)),
                            DeptId = Convert.ToInt32(reader.GetValue(4))
                        });
                    }
                }
            }
            return attendance;
        }

        //================== End Upload Excel Sheet ==================//
        //============================================================//



        //============================================================//
        //===================== Start Validation =====================//

        public IActionResult CheckAbsenceTimeWithAttendanceTime(DateTime AttendanceTime, DateTime AbsenceTime)
        {
            if (AttendanceTime < AbsenceTime)
            { return Json(true); }
			else if (AbsenceTime.Hour == 0 && AbsenceTime.Minute == 0)
            { return Json(true); }
            return Json(false);
        }

        //============================================================//
        //====================== End Validation ======================//


    }
}
