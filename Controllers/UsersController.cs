using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.DTO.UsersDTO;
using UGB.MVC.Entities;
using UGB.MVC.Mapper;
namespace UGB.MVC.Controllers
{
    //realizamos la inyección de dependencias del validador de CreateUserDTO
    public class UsersController(IValidator<CreateUserDTO> createUserDTOValidator) : Controller
    {
        public IActionResult Index()
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
            IEnumerable<UserDTO> userDTOs = CustomMapper<UserDTO>.Map(users);
            return Ok(userDTOs);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateUserDTO createUserDTO)
        {
            //validamos el objeto que recibimos en el body de la petición
            var validation = createUserDTOValidator.Validate(createUserDTO);

            //si el objeto no es valido retornamos un BadRequest con los errores de validación
            if(!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            //mapeamos el objeto CreateUserDTO a la entidad users para poder guardarlo en la base de datos
            users user = CustomMapper<users>.Map(createUserDTO);
            return Ok(user);
        }
    }
}