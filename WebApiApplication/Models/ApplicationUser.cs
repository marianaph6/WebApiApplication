using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApiApplication.Models
{
    public class ApplicationUser: IdentityUser
    {

        [Required(ErrorMessage = "La cedula es requerida.")]
        [Key]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; }
       


    }
}
