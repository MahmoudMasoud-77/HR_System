using HRSystem.Models;
using HRSystem.Repository;

namespace HRSystem.ViewModels
{
    public class EmpWithDepVM
    {
        public Employee EmpModel { get; set; }
        public Department DepModel { get; set; }

        public List<Employee> Emplist { get; set; }

        public List<Department> DepartmentList { get; set; }
    }
}
