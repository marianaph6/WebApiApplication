using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiApplication.Context;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;  

        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;

        }

        [HttpPost("ToAssingUserRole")]
        //Asignar a un usuario a un rol
        public async Task<ActionResult> ToAssingUserRole(EditRoleDTO editRoleDTO)
        {
            var user= context.Users.FirstOrDefault(x => x.Cedula == editRoleDTO.UserCedula);

            if (user == null)
            {
                return NotFound();
               
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.AddToRoleAsync(user,editRoleDTO.RoleName);
            return Ok();
        }

        //Remover a un usuario de un rol
        public async Task<ActionResult> ToRemoveUserRole(EditRoleDTO editRoleDTO)
        {
            var user = context.Users.FirstOrDefault(x => x.Cedula == editRoleDTO.UserCedula);

            if (user == null)
            {
                return NotFound();
            }

            await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(user, editRoleDTO.RoleName);
            return Ok();
        }
    }
}
