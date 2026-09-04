using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;

namespace UGB.MVC.Aplicaciones.Seguras.Repositories
{
    public class RolesRepository(StoreCTX ctx) : IRolesRepository
    {
        public async Task<IEnumerable<roles>> GetAll()
        {
            return await ctx.roles.ToListAsync();
        }
    }
}