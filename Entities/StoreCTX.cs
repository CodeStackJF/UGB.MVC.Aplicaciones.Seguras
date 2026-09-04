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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Con esto habilitamos la configuración de las entidades a través de clases de configuración separadas en la carpeta DatabaseConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreCTX).Assembly);
        }

        public DbSet<users> users {get; set;}
        public DbSet<roles> roles {get; set;}
        public DbSet<users_roles> users_roles {get; set;}
    }
}