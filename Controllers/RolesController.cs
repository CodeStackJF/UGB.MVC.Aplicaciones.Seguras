using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;

namespace UGB.MVC.Aplicaciones.Seguras.Controllers
{
    public class RolesController(IRolesRepository rolesRepository) : Controller
    {
        public async Task<IActionResult> Index( )
        {
            return Ok(await rolesRepository.GetAll());
        }
    }
}