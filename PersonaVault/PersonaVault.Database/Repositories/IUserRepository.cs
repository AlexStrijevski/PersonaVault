using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Database.Models;

namespace PersonaVault.Database.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string username);
        Task CreateUser(NewUserDTO newUser);
        Task<User> GetUser(string username);
        Task<User> GetUserWithPersonalDetails(Guid userId);
        Task<User> GetFullUserData(Guid userId);
        Task DeleteUser(User user);
        Task<User> GetFullUserData(string username);
    }
}