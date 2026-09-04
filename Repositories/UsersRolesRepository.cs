using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;

namespace UGB.MVC.Aplicaciones.Seguras.Repositories
{
    public class UsersRolesRepository(StoreCTX ctx) : IUsersRolesRepository
    {
        public async Task<users_roles> Insert(users_roles usersRoles)
        {
            usersRoles.role = null!;
            ctx.users_roles.Add(usersRoles);
            await ctx.SaveChangesAsync();
            return usersRoles;
        }

    }
}