using HRSystem.Models;

namespace HRSystem.Repository
{
	public interface IDepartment
	{
		List<Department> GetAll();

		Department GetByID(int id);

		void Insert(Department newDepartment);

		void Update(int id, Department newDepartment);

		void Delete(int id);
	}
}
