using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiApplication.Context;
using WebApiApplication.Models;
using WebApiApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Habilitar CORS
builder.Services.AddCors(options =>
{
    //Añadir politicas de seguridad
    options.AddPolicy("cors",
        b =>
            b.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
});


//Configurar mapeo de entidades a DTO
//builder.Services.AddAutoMapper(configuration =>
//    {
//        configuration.CreateMap<User, UserDTO>();
//        configuration.CreateMap<UserCreationDTO, User>().ReverseMap();  
//    },typeof(Program));

//Metodo que sirve para configurar un conexto de datos en ASP.NET Core
//Se obtiene de uno de los proveedores de conf el connection string


builder.Services.AddScoped<HashService>();
builder.Services.AddDataProtection();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"])),
                     ClockSkew = TimeSpan.Zero
                 });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = $"/account/login";
//    options.LogoutPath = $"/account/logout";
//    options.AccessDeniedPath = $"/account/accessDenied";
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("cors");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
