using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UGB.MVC.Aplicaciones.Seguras.Entities;
using UGB.MVC.Aplicaciones.Seguras.Interfaces;
using UGB.MVC.Entities;

namespace UGB.MVC.Aplicaciones.Seguras.Repositories
{
    public class UsersRepository(StoreCTX ctx) : IUsersRepository
    {
        public async Task<bool> Authenticate(string email, string password, string salt)
        {
            return await ctx.users.Where(x=>x.email == email && x.password == password && x.salt == salt).AnyAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var user = await ctx.users.FindAsync(id);
            if(user == null) return false;
            ctx.Remove(user);
            return await ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await ctx.users.AnyAsync(x=>x.email == email);
        }

        public async Task<users> Get(int id)
        {
            return await ctx.users.FindAsync(id);
        }

        public async Task<IEnumerable<users>> GetAll()
        {
            return await ctx.users.ToListAsync();
        }

        public async Task<users> GetByEmail(string email)
        {
            return await ctx.users.Where(x=>x.email == email).FirstOrDefaultAsync();
        }

        public async Task<users> Insert(users user)
        {
            ctx.users.Add(user);
            await ctx.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Update(int id, users user)
        {
            users _user = await ctx.users.FindAsync(id);
            _user.first_name = user.first_name;
            return await ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePassword(int id, string password, string salt)
        {
             users _user = await ctx.users.FindAsync(id);
             _user.password = password;
             _user.salt = salt;
             return await ctx.SaveChangesAsync() > 0;
        }
    }
}