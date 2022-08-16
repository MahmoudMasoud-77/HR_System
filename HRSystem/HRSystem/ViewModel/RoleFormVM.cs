using System.ComponentModel.DataAnnotations;

namespace HRSystem.ViewModel
{
    public class RoleFormVM
    {
        [Required , StringLength(150)]
        public string Name { get; set; }
    }
}
