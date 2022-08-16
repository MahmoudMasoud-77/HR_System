using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required , MaxLength(150)]
        public string Name { get; set; }

    }
}
