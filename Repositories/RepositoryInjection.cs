using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;

namespace UGB.MVC.Aplicaciones.Seguras.Repositories
{
    public static class RepositoryInjection
    {
        public static IServiceCollection AddRepositoryInjection(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            return services;
        }
    }
}