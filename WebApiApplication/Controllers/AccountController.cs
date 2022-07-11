using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        //Recibir userInfo (user y password)
        // Instanciar application user para pasar info y crear el usuario
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO model)
        {
            var result = new IdentityResult();

            if (ModelState.IsValid)
            {

                var userCheck = await _userManager.FindByEmailAsync(model.Email);
                if (userCheck == null)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido, Cedula = model.Cedula };
                    result = await _userManager.CreateAsync(user, model.Password);
                }
                else
                {
                    ModelState.AddModelError("message", "El email ya existe");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                return BadRequest(ModelState);
            }

            if (result.Succeeded)
            {
                return Ok(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserLoginDTO model)
        {

            var result = new Microsoft.AspNetCore.Identity.SignInResult();

            if (ModelState.IsValid)
            {
                result = await _signInManager.PasswordSignInAsync(model.Email,
                                                                  model.Password,
                                                                  isPersistent: false, 
                                                                  lockoutOnFailure: false);
            }

            else
            {
                ModelState.AddModelError("message", "Invalid login model data");
            }

            if (result.Succeeded)
            {

                var user=await _userManager.FindByNameAsync(model.Email);
                var roles= await _userManager.GetRolesAsync(user);
                return BuildToken(model,roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }


        }

        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("login", "account");
        //}

        //Construir token
        //Instanciar Claims (info confiable que  viaja en el token)

        private UserToken BuildToken(UserLoginDTO userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {

                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                //new Claim("miValor", "Lo que yo quiera"),

                //JTI → Identificar de manera unica un token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            //Construir llave simetrica de seguridad para garantizar la autenticidad de la info que se está recibiendo

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
