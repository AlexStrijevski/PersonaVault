using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;

namespace PersonaVault.Database.Repositories
{
    public interface IPersonalDetailsRepository
    {
        Task CreateAndAddPersonalDetailsToUser(EncryptedPersonalDetailsDTO request, User user);
        Task UpdateName(string newName, User user);
        Task UpdateLastName(string newName, User user);
        Task UpdatePersonalCode(byte[] personalCode, User user);
        Task UpdateEmail(byte[] email, User user);
        Task UpdatePhoneNumber(byte[] phoneNumber, User user);
    }
}