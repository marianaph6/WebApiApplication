using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiApplication.Context;
using WebApiApplication.Entities;
using Microsoft.Extensions.Logging;
using WebApiApplication.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApiApplication.Controllers
{

    //-nombre debe terminar en controller
    //-debe heredar de controller base
    //-API controller es un atributo que trae un conjunto de convenciones para simplificar el codigo de las acciones
    //-Regla de ruteo
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        //private readonly ILogger<UsersController> logger;
        private readonly IMapper mapper;

        //Con el controlador se puede hacer el CRUD hacia la bd
        //Inyectar el ApplicationDbContext en la clase users controller

        //Intyectar servicio de AutoMapper en el controlador para implementar los DTOs
        //ILogger<UsersController> logger
        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            //this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        //Ienumerable → Colección/Lista
        //Action result → Posibles respuestas que se pueden retornar 
        public ActionResult<IEnumerable<UserDTO>> Get()
        {
            //logger.LogInformation("Looking users");
            var users = context.Users.ToList();
            var usersDTO = mapper.Map<List<UserDTO>>(users);

            return usersDTO;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<UserDTO> Get(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                //logger.LogWarning($"The user with Id {id} not found");
                return NotFound();
            }

            var userDTO = mapper.Map<UserDTO>(user);
            return userDTO;
        }

        //Las entidades sirven para representar uana tabla en la BD no deberia ser usada como valor de entrada

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserCreationDTO userCreation)
        {

            var user = mapper.Map<User>(userCreation);
            context.Add(user);
            await context.SaveChangesAsync();
            //Ser consistente con los tipos de datos de retorno de los metodos
            var userDTO = mapper.Map<UserDTO>(user);
            return new CreatedAtRouteResult("GetUser", new { id = user.Id }, userDTO);

        }

        [HttpPut("{id}")]
        //Id del url
        public async Task<ActionResult> Put(int id, [FromBody] UserCreationDTO userUpdate)
        {
            var user = mapper.Map<User>(userUpdate);
            user.Id = id;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserCreationDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = mapper.Map<UserCreationDTO>(user);

            jsonPatchDocument.ApplyTo(userDTO, ModelState);

            mapper.Map(userDTO, user);

            //Validar reglas de validación
            var isValid = TryValidateModel(user);

            if (!isValid)
            {
                BadRequest(ModelState);
            }
            
            await context.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id) 
        {
            //Buscar usuario en la BD
            var userId = await context.Users.Select(x =>x.Id).FirstOrDefaultAsync(x => x == id);

            if (userId == default(int))
            {
                return NotFound();
            }

            context.Remove(new User { Id= userId });
            context.SaveChanges();
            return NoContent();

        }
    }
}
