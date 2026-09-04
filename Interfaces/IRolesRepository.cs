using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGB.MVC.Aplicaciones.Seguras.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Interfaces
{
    public interface IRolesRepository
    {
        public Task<IEnumerable<roles>> GetAll();
    }
}