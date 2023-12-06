using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Responses;

namespace PersonaVault.Business.Managers
{
    public interface IPersonalDetailsManager
    {
        Task<ActionResponse> CreatePersonalDetails(NewPersonalDetailsDTO request, Guid userId);
        Task<ActionResponse> UpdateName(string newName, Guid userId);
        Task<ActionResponse> UpdateLastName(string newLastName, Guid userId);
        Task<ActionResponse> UpdatePersonalCode(string personalCode, Guid userId);
        Task<ActionResponse> UpdatePhoneNumber(string newPhoneNumber, Guid userId);
        Task<ActionResponse> UpdateEmailAddress(string newEmail, Guid userId);
    }
}