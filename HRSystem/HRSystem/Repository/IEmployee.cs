using HRSystem.Models;

namespace HRSystem.Repository
{
    public interface IEmployee
    {
        List<Employee> GetAll();
        List<Employee> GetByName(string id);

        Employee GetByID(int id); 
        Employee GetByIDWithDep(int id);
        void Insert(Employee newEmployee);

        void Update(int id, Employee newEmployee);

        void Delete(int id);
    }
}
