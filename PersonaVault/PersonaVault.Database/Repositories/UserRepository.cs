using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Enums;
using PersonaVault.Contracts.Requests;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly PersonaVaultContext _context;

        public UserRepository(PersonaVaultContext context)
        {
            _context = context;
        }

        public async Task CreateUser(NewUserDTO newUser)
        {
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = newUser.Username,
                PasswordHash = newUser.PasswordHash,
                PasswordSalt = newUser.PasswordSalt,
                Role = Role.User
            });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<User> GetUser(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserWithPersonalDetails(Guid userId)
        {
            return await _context.Users.Include(p => p.PersonalDetails).SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetFullUserData(Guid userId)
        {
            return await _context.Users
                .Include(p => p.PersonalDetails)
                .ThenInclude(a => a.AddressDetails)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetFullUserData(string username)
        {
            return await _context.Users
                .Include(p => p.PersonalDetails)
                .ThenInclude(a => a.AddressDetails)
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task DeleteUser(User user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
