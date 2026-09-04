using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Entities;
namespace UGB.MVC.Aplicaciones.Seguras.DatabaseConfiguration
{
    public class RolesConfiguration : IEntityTypeConfiguration<roles>
    {
        public void Configure(EntityTypeBuilder<roles> builder)
        {
            //se define la llave primaria de la tabla roles
            builder.HasKey(x=>x.id);
        }
    }
}