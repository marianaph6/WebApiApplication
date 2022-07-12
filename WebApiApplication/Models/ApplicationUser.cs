using Microsoft.AspNetCore.Identity;

namespace WebApiApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public long Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

    

    }
}
