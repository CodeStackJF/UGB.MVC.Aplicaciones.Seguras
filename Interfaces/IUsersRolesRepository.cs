using UGB.MVC.Aplicaciones.Seguras.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Interfaces
{
    public interface IUsersRolesRepository
    {
        public Task<users_roles> Insert(users_roles usersRoles);
    }
}