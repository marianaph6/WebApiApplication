﻿using Microsoft.AspNetCore.Identity;
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

        

        

    }
}

