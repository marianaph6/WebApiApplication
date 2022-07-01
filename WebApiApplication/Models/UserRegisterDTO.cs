using System.ComponentModel.DataAnnotations;

namespace WebApiApplication.Models
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "La Cedula es requerida")]
        [Display(Name = "Cedula")]
        public long Cedula { get; set; }

        [Required(ErrorMessage ="El Nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required (ErrorMessage ="El Apellido es requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required (ErrorMessage ="El email es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required (ErrorMessage ="La confirmación de la contraseña es requerida")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden")]
        public string ConfirmPassword { get; set; }

    }
}
