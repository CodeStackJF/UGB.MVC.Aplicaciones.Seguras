using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Aplicaciones.Seguras.Helper;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.DTO.UsersDTO;
using UGB.MVC.Entities;
using UGB.MVC.Mapper;
namespace UGB.MVC.Controllers
{
    //Globalmente, todos los métodos de este controlador requieren autenticación para acceder a sus acciones
    //Solo en los métodos que se requiera un rol específico se puede agregar el atributo [Authorize(Roles = "Rol")]
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    //realizamos la inyección de dependencias del validador de CreateUserDTO
    public class UsersController(
                IValidator<CreateUserDTO> createUserDTOValidator,
                IUsersRepository usersRepository,
                IUsersRolesRepository usersRolesRepository
            ) : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserDTO createUserDTO)
        {
            //validamos el objeto que recibimos en el body de la petición
            var validation = createUserDTOValidator.Validate(createUserDTO);

            //si el objeto no es valido retornamos un BadRequest con los errores de validación
            if(!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            //verificamos si el correo existe antes de registrar
            if(await usersRepository.EmailExists(createUserDTO.email))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Este correo ya se encuentra registrado.",
                    StatusCode = 400
                });
            }

            //Creamos el hash de la contraseña y el salt para guardarlo en la base de datos
            HashedPassword hashedPassword = HashHelper.Hash(createUserDTO.password);

            //mapeamos el objeto CreateUserDTO a la entidad users para poder guardarlo en la base de datos
            users user = CustomMapper<users>.Map(createUserDTO);

            //asignamos la contraseña hasheada y el salt al objeto users
            user.password = hashedPassword.Password;
            user.salt = hashedPassword.Salt;

            //insertamos el nuevo usuario en la base de datos y retornamos el objeto mapeado a UserDTO
            user = await usersRepository.Insert(user);
            await usersRolesRepository.Insert(new users_roles()
            {
                user_id = user.id,
                role_id = 2 //asignamos el rol de usuario por defecto
            });
            user = await usersRepository.Get(user.id);
            return Ok(CustomMapper<UserDTO>.Map(user));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await usersRepository.GetAll();
            return Ok(CustomMapper<UserDTO>.Map(users));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ListAll(int id)
        {
            ViewBag.Users = await usersRepository.GetAll();
            return View();
        }

        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> ShowAuthenticated(int id)
        {
            string email = User.GetProperty(ClaimTypes.Email);
            ViewBag.User = await usersRepository.GetByEmail(email);
            return View();
        }

        /*
            CODIGO ANTERIOR
            public IAcionResult Index()
            {
                //Creamos una lista de usuarios ya que no tenemos base de datos
            IEnumerable<users> users = new List<users>()
            {
                new users()
                {
                    id = 1,
                    first_name = "Jose",
                    last_name = "Jimenez",
                    email = "jose@ugb.edu.sv",
                    password = "123456",
                    salt = "123456",
                    created_on = DateTime.Now,
                },
                new users()
                {
                    id = 2,
                    first_name = "Pedro",
                    last_name = "Rivas",
                    email = "pedro@ugb.edu.sv",
                    password = "123456",
                    salt = "123456",
                    created_on = DateTime.Now,
                }
            };
            //esta es una forma de conversión manual pero causaría codigo repetitivo si se tiene que hacer en varios lugares, por eso se crea un mapper para hacer la conversión de manera genérica
            /*List<UserDTO> usersDTOs = users.Select(x=> new UserDTO()
            {
                id = x.id,
                firstName = x.first_name,
                lastName = x.last_name,
                email = x.email
            }).ToList();
            
            //Creamos un usuario para probar el mapeo de un solo objeto
            users user = new users()
            {
                id = 3,
                first_name = "Jhon",
                last_name = "Doe",
            };

            //Realizamos el mapeo de un solo objeto
            UserDTO userDTO = CustomMapper<UserDTO>.Map(user);*/

            //realizamos el mapeo de una lista de objetos
           /*     IEnumerable<UserDTO> userDTOs = CustomMapper<UserDTO>.Map(users);
                return Ok(userDTOs);
            }
        */
    }
}