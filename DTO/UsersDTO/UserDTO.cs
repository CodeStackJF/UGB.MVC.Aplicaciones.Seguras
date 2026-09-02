using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGB.MVC.DTO.UsersDTO
{
    //Listar o leer datos
    public class UserDTO
    {
        public int id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}