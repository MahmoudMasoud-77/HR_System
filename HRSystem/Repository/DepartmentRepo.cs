using HRSystem.Data;
using HRSystem.Models;

namespace HRSystem.Repository
{
    public class DepartmentRepo:IDepartment
    {
        ApplicationDbContext context;
        public DepartmentRepo(ApplicationDbContext db)
        {
            context = db;
        }
        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }
        public Department GetByID(int id)
        {
            Department dept = context.Departments.FirstOrDefault(e => e.Id == id);
            return dept;
        }
        public void Insert(Department newDepartment)
        {
            context.Departments.Add(newDepartment);
            context.SaveChanges();
        }
        public void Update(int id, Department dept)
        {
            Department oldDept = GetByID(id);

            oldDept.Name = dept.Name;

            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Department dept = GetByID(id);
            context.Departments.Remove(dept);
            context.SaveChanges();
        }
    }
}
