using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGB.MVC.Aplicaciones.Seguras.DTO.RolesUsersDTO;
using UGB.MVC.Aplicaciones.Seguras.Entities;

namespace UGB.MVC.DTO.UsersDTO
{
    //Listar o leer datos
    public class UserDTO
    {
        public int id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public IEnumerable<RolesUsersDTO> roles { get; set; } = new List<RolesUsersDTO>();
    }
}