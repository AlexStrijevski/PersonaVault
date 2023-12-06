using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;

namespace PersonaVault.Business.Services
{
    public interface IPersonalDetailsRequirementsValidator
    {
        ActionResponse ValidateRequirementsForPersonalDetailsCreation(NewPersonalDetailsDTO data, User user);
        ActionResponse ValidateRequirementsForNameUpdate(User user);
        ActionResponse ValidateRequirementsForLastNameUpdate(User user);
        ActionResponse ValidateRequirementsForPersonalCodeUpdate(string personalCode, User user);
        ActionResponse ValidateRequirementsForPhoneNumberUpdate(string phoneNumber, User user);
        ActionResponse ValidateRequirementsForEmailUpdate(string emailAddress, User user);
    }
}