using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;

namespace PersonaVault.Contracts.Mappers
{
    public interface IUserDataMapper
    {
        NewUserDTO MapToNewUserDTO(byte[] passwordHash, byte[] passwordSalt, RegisterUserRequest request);
    }
}