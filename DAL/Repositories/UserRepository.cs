using DAL.Context;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private VocaquillDbContext _context;

        public UserRepository(VocaquillDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Login = user.Login;
                existingUser.Password = new PasswordHasher<User>().HashPassword(user, user.Password);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
