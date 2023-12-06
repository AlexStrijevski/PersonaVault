using PersonaVault.Contracts.Enums;

namespace PersonaVault.Business.Security
{
    public interface IJwtService
    {
        string GetJwtToken(Guid id, Role role);
    }
}