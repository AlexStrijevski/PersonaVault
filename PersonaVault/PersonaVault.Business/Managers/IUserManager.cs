using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;

namespace PersonaVault.Business.Managers
{
    public interface IUserManager
    {
        Task<ActionResponse> RegisterUser(RegisterUserRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<FullUserData> GetFullUserData(Guid userId);
        Task<ActionResponse> DeleteUser(string username);
    }
}