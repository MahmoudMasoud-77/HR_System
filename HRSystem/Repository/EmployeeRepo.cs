using HRSystem.Data;
using HRSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Repository
{
    public class EmployeeRepo : IEmployee
    {
        ApplicationDbContext context;
        public EmployeeRepo(ApplicationDbContext _context)
        {
            context = _context;
        }
        public List<Employee> GetAll()
        {
            return context.Employees.ToList();
        }

        public Employee GetByID(int id)
        {
            Employee employee = context.Employees.FirstOrDefault(x => x.Id == id);
            return employee;
        }
        public Employee GetByIDWithDep(int id)
        {
            Employee employee = context.Employees.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
            return employee;
        }
        public List<Employee> GetByName(string id)
        {
            return context.Employees.Where(x => x.Name.ToLower().Contains(id.ToLower())).ToList();
        }

        public void Insert(Employee newEmployee)
        {
            context.Employees.Add(newEmployee);
            context.SaveChanges();
        }

        public void Update(int id, Employee newEmployee)
        {
            Employee oldEmployee=GetByID(id);

            oldEmployee.Name=newEmployee.Name;
            oldEmployee.Gender=newEmployee.Gender;
            oldEmployee.Address=newEmployee.Address;
            oldEmployee.Phone=newEmployee.Phone;
            oldEmployee.Nationality=newEmployee.Nationality;
            oldEmployee.BirthDay=newEmployee.BirthDay;
            oldEmployee.NationalId=newEmployee.NationalId;
            oldEmployee.Salary=newEmployee.Salary;
            oldEmployee.AttendanceTime=newEmployee.AttendanceTime;
            oldEmployee.AbsenceTime=newEmployee.AbsenceTime;
            oldEmployee.HireDate=newEmployee.HireDate;

            context.SaveChanges();


        }
        public void Delete(int id)
        {
            Employee employee=GetByID(id);
            context.Employees.Remove(employee);
            context.SaveChanges();
        }
    }
}
