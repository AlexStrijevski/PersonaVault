using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;

namespace PersonaVault.Business.Requirements
{
    internal interface IUserDataRequirements
    {
        Task<bool> CheckIfUserDetailsMeetRequirements(RegisterUserRequest request);

    }
}