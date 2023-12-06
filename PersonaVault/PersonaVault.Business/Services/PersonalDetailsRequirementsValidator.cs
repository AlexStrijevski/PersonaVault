using PersonaVault.Business.Requirements;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Services
{
    internal class PersonalDetailsRequirementsValidator : IPersonalDetailsRequirementsValidator
    {
        private readonly IPersonalDetailsRequirements _personalDetailsRequirements;

        public PersonalDetailsRequirementsValidator(IPersonalDetailsRequirements personalDetailsRequirements)
        {
            _personalDetailsRequirements = personalDetailsRequirements;
        }

        public ActionResponse ValidateRequirementsForPersonalDetailsCreation(NewPersonalDetailsDTO data, User user)
        {
            if (user.PersonalDetails != null)
                return new ActionResponse(false, 400, "Personal Details already exist");

            if(!_personalDetailsRequirements.DoesPersonalDetailsMeetRequirements(data))
                return new ActionResponse(false, 400, "Some fields are not valid, please check input data");

            return new ActionResponse(true);
        }

        public ActionResponse ValidateRequirementsForNameUpdate(User user)
        {
            if (user.PersonalDetails == null)
                return new ActionResponse(false, 404, "Please add Personal Details first, in order to update specific field");

            return new ActionResponse(true);
        }

        public ActionResponse ValidateRequirementsForLastNameUpdate(User user)
        {
            if (user.PersonalDetails == null)
                return new ActionResponse(false, 404, "Please add Personal Details first, in order to update specific field");

            return new ActionResponse(true);
        }

        public ActionResponse ValidateRequirementsForPersonalCodeUpdate(string personalCode, User user)
        {
            if (user.PersonalDetails == null)
                return new ActionResponse(false, 404, "Please add Personal Details first, in order to update specific field");

            if(!_personalDetailsRequirements.DoesPersonalCodeMeetRequirements(long.Parse(personalCode)))
                return new ActionResponse(false, 400, "Personal Code does not meet requirements");

            return new ActionResponse(true);
        }

        public ActionResponse ValidateRequirementsForPhoneNumberUpdate(string phoneNumber, User user)
        {
            if (user.PersonalDetails == null)
                return new ActionResponse(false, 404, "Please add Personal Details first, in order to update specific field");

            if (!_personalDetailsRequirements.DoesPhoneNumberMeetRequirements(long.Parse(phoneNumber)))
                return new ActionResponse(false, 400, "Phone Number does not meet requirements");

            return new ActionResponse(true);
        }

        public ActionResponse ValidateRequirementsForEmailUpdate(string emailAddress, User user)
        {
            if (user.PersonalDetails == null)
                return new ActionResponse(false, 404, "Please add Personal Details first, in order to update specific field");

            if (!_personalDetailsRequirements.DoesEmailMeetRequirements(emailAddress))
                return new ActionResponse(false, 400, "Email Address does not meet requirements");

            return new ActionResponse(true);
        }
    }
}
