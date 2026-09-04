using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Entities
{
    public class StoreCTX : DbContext
    {
        public StoreCTX(DbContextOptions<StoreCTX> options) : base(options)
        {
        }

        public DbSet<users> users {get; set;}
        public DbSet<roles> roles {get; set;}
    }
}