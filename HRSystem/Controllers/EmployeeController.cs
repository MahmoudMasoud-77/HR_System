using HRSystem.Constants;
using HRSystem.Models;
using HRSystem.Repository;
using HRSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployee EmployeeRepo;
        private readonly IDepartment DepartmentRepo;

        public EmployeeController(IEmployee _EmployeeRepo,IDepartment _DepartmentRepo)
        {
            this.EmployeeRepo = _EmployeeRepo;
            this.DepartmentRepo = _DepartmentRepo;
        }


        // GET: EmployeeController
        [Authorize(Permissions.Employee.View)]
        public ActionResult Index()
        {
            EmpWithDepVM VM = new EmpWithDepVM();
            VM.Emplist= EmployeeRepo.GetAll();
            VM.DepartmentList= DepartmentRepo.GetAll();

            //List<Employee> Employees = EmployeeRepo.GetAll();
            //return View(Employees);
            return View(VM);
        }
       
        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            EmpWithDepVM VM = new EmpWithDepVM();
            VM.EmpModel = EmployeeRepo.GetByIDWithDep(id);
            if (VM.EmpModel != null)
                return View(VM);

            return NotFound();

            //ViewData["DepList"] = DepartmentRepo.GetByID(id);
            //Employee employee = EmployeeRepo.GetByID(id);
            //if (employee != null)
            //    return View(employee);

            //return NotFound();
        }

        // GET: EmployeeController/Create
        [Authorize(Permissions.Employee.Create)]
        public ActionResult Add()
        {
            ViewData["DepList"] = DepartmentRepo.GetAll();
            return View(new Employee());
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAdd(Employee Emp)
        {
            if (ModelState.IsValid)
            {
                EmployeeRepo.Insert(Emp);
                TempData["message"] = "Added Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["DepList"] = DepartmentRepo.GetAll();
                TempData["message"] = "There's a problem. Please Check data entered again.";
                return View("Add",Emp);
            }
        }

        // GET: EmployeeController/Edit/5
        [Authorize(Permissions.Employee.Edit)]
        public ActionResult Edit(int id)
        {
            Employee EmpModel = EmployeeRepo.GetByID(id);
            ViewData["DepList"] = DepartmentRepo.GetAll();
            return View(EmpModel);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEdit(int id, Employee NewEmp)
        {
            if (ModelState.IsValid)
            {
                Employee OldEmp = EmployeeRepo.GetByID(id);
                if (OldEmp != null)
                {
                    EmployeeRepo.Update(id, NewEmp);
                    return RedirectToAction("Index");
                }
            }
            ViewData["DepList"] = DepartmentRepo.GetAll();
            return View("Edit", NewEmp);
        }


        // POST: EmployeeController/Delete/5
        
        public ActionResult Delete(int id)
        {
            //Employee Emp = EmployeeRepo.GetByID(id);
            //if (Emp == null)
            //{
            //    return NotFound();
            //}

            EmployeeRepo.Delete(id); 

            return RedirectToAction("Index");
        }





    }
}


//// GET: EmployeeController/Delete/5
//[HttpGet]
//public ActionResult Delete(int id)
//{
//    if (id == null || id == 0)
//    {
//        return NotFound();
//    }
//    var obj = EmployeeRepo.GetByID(id);
//    if (obj == null)
//    {
//        return NotFound();
//    }

//    return View(obj);
//}


//public IActionResult Index(string name)
//{
//    if(!string.IsNullOrEmpty(name))
//    {
//        List<Employee> Employeeslis = EmployeeRepo.GetByName(name);
//        return View(Employeeslis);
//    }
//    List<Employee> Employees = EmployeeRepo.GetAll();
//    return View(Employees);

//}

//public IActionResult GetByName(string name)
//{
//    List<Employee> Employee = EmployeeRepo.GetByName(name);
//    return Json(employee);
//}