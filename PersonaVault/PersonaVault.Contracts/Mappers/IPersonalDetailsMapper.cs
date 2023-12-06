using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;

namespace PersonaVault.Contracts.Mappers
{
    public interface IPersonalDetailsMapper
    {
        NewPersonalDetailsDTO MapToPersonalDetailsDTO(CreatePersonalDetailsRequest request, byte[] picture);
    }
}