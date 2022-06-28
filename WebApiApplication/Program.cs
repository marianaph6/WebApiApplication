using Microsoft.EntityFrameworkCore;
using WebApiApplication.Context;
using WebApiApplication.Entities;
using WebApiApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar mapeo de entidades a DTO
builder.Services.AddAutoMapper(configuration =>
    {
        configuration.CreateMap<User, UserDTO>();
        configuration.CreateMap<UserCreationDTO, User>().ReverseMap();  
    },typeof(Program));

//Metodo que sirve para configurar un conexto de datos en ASP.NET Core
//Se obtiene de uno de los proveedores de conf el connection string
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
