using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;

namespace PersonaVault.Business.Services
{
    public interface IAddressDetailsRequirementsValidator
    {
        ActionResponse ValidateRequirementsForAddressDetailsCreation(User user);
        ActionResponse ValidateRequirementsForFieldUpdate(User user);
    }
}