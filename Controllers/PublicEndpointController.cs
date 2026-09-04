using System.Formats.Asn1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.DTO.UsersDTO;
using UGB.MVC.Entities;
using UGB.MVC.Mapper;

namespace UGB.MVC.Aplicaciones.Seguras.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //Aplicamos la politica que toda peticion realizada debe incluir en el header la propiedad X-API-KEY con el valor almacenado en el appsettings.json
    [Authorize(Policy="API-KEY-POLICY")]
    public class PublicEndpointController(IUsersRepository usersRepository) : ControllerBase
    {
        public async Task<IActionResult> Get()
        {
            IEnumerable<users> users = await usersRepository.GetAll();
            return Ok(CustomMapper<UserDTO>.Map(users));
        }
    }
}