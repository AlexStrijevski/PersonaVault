using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;

namespace PersonaVault.Business.Requirements
{
    public interface IPersonalDetailsRequirements
    {
        bool DoesPersonalDetailsMeetRequirements(NewPersonalDetailsDTO data);
        bool DoesEmailMeetRequirements(string email);
        bool DoesPersonalCodeMeetRequirements(long personalCode);
        bool DoesPhoneNumberMeetRequirements(long phoneNumber);
    }
}