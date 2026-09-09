using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.Aplicaciones.Seguras.DTO.RolesDTO;
using UGB.MVC.Mapper;
using Microsoft.AspNetCore.Authorization;
namespace UGB.MVC.Aplicaciones.Seguras.Controllers
{
    //Globalmente, todos los métodos de este controlador requieren autenticación como administrador para acceder a sus acciones
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class RolesController(IRolesRepository rolesRepository) : Controller
    {        
      public async Task<IActionResult> Index( )
        {
            return Ok(await rolesRepository.GetAll());
        } 
    }
}