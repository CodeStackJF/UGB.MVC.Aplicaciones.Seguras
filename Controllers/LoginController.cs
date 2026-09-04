using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Aplicaciones.Seguras.DTO.UsersDTO;
using UGB.MVC.Aplicaciones.Seguras.Helper;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Controllers
{
    public class LoginController(IUsersRepository usersRepository) : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Authenticate([FromBody] LoginUserDTO loginUserDTO)
        {
            users user = await usersRepository.GetByEmail(loginUserDTO.email);
            if(user == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            bool validCredentials = HashHelper.CheckHash(loginUserDTO.password, user.password, user.salt);
            if(!validCredentials)
            {
                return BadRequest("Las credenciales no son válidas.");
            }

            return Ok();
        }
    }
}