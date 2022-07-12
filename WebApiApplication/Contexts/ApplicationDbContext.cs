using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiApplication.Models;

namespace WebApiApplication.Context
{
    //Contexto de datos
    //Se configuran las tablas de la BD
    //Configuración de campos a nivel de tabla
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        ////crear constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var rolAdmin = new IdentityRole()
            {
                Id = "6fa5354a-02f6-4d7e-b424-c423a577132a",
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var rolTrainee = new IdentityRole()
            {
                Id = "f2b4503a-d4d9-4513-aed9-1d17227bfb4f",
                Name = "Trainee",
                NormalizedName = "Trainee"
            };

            
            builder.Entity<IdentityRole>().HasData(rolAdmin);
            builder.Entity<IdentityRole>().HasData(rolTrainee);

            base.OnModelCreating(builder);
        }





    }
}

