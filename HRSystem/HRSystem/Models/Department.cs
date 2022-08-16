namespace HRSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relations
        public virtual List<Employee>? Employees { get; set; }
    }
}
