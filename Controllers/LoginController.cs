using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using UGB.MVC.Aplicaciones.Seguras.DTO.UsersDTO;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Aplicaciones.Seguras.Helper;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(  
        IUsersRepository usersRepository,
        IValidator<LoginUserDTO> loginUserDTOValidator,
        IConfiguration config
    ) : Controller
    {
        //Esta acción es pública, no requiere autenticación para acceder a ella
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Authenticate([FromBody] LoginUserDTO loginUserDTO)
        {
            //Aplicamos la validación de los datos del login
            var validation = loginUserDTOValidator.Validate(loginUserDTO);
            if(!validation.IsValid)
            {
                
                return BadRequest(validation.Errors);
            }

            //buscamos el usuario por su correo
            users user = await usersRepository.GetByEmail(loginUserDTO.email);
            if(user == null)
            {
                throw new HttpRequestException("El usuario o la contraseña no son válidas.");
            }
            
            //validamos las credenciales
            bool validCredentials = HashHelper.CheckHash(loginUserDTO.password, user.password, user.salt);
            if(!validCredentials)
            {
                throw new HttpRequestException("El usuario o la contraseña no son válidas.");
            }

            //iniciamos sesión con jwt
            var identity = new ClaimsIdentity();
            
            //Almacenamos los datos en la sesión, nunca se deben guardar datos sensibles
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.email));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.email));
            identity.AddClaim(new Claim("UserId", user.id.ToString()));
            identity.AddClaim(new Claim("UserName", user.first_name + " " + user.last_name));

            //Registramos cada uno de los roles
            foreach(var role in user.users_roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.role.description));
            }

            //Definimos la fecha de expiración que es de 4 horas
            DateTime sessionExpirationDate = DateTime.Now.AddHours(4);

            string JWT_TOKEN = config.GetValue<string>("JWT_TOKEN")!;
            byte[] bytesKey = Encoding.ASCII.GetBytes(JWT_TOKEN);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = sessionExpirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytesKey), SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.Now
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken createdToken = tokenHandler.CreateToken(tokenDescriptor);
                string bearerToken = tokenHandler.WriteToken(createdToken);

            return Ok(new
            {
                bearerToken = bearerToken,
                user.email,
                roles = user.users_roles.Select(x=>x.role.description).ToList()
            });
        }
    }
}