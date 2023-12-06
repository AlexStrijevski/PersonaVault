using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Services
{
    internal class AddressDetailsRequirementsValidator : IAddressDetailsRequirementsValidator
    {
        public ActionResponse ValidateRequirementsForAddressDetailsCreation(User user)
        {
            if (!DoesUserHavePersonalDetails(user))
            {
                return new ActionResponse(false, 404, "You have to add personal details first, in order  to add address details");
            }
            else if (DoesUserHaveAddressDetails(user))
            {
                return new ActionResponse(false, 400, "Address details already exist");
            }
            else
            {
                return new ActionResponse(true);
            }
        }

        private bool DoesUserHavePersonalDetails(User user)
        {
            return user.PersonalDetails != null;
        }

        private bool DoesUserHaveAddressDetails(User user)
        {
            return user.PersonalDetails.AddressDetails != null;
        }

        public ActionResponse ValidateRequirementsForFieldUpdate(User user)
        {
            if (!DoesUserHavePersonalDetails(user))
            {
                return new ActionResponse(false, 404, "You have to add personal details first and add address details, in order to update specific fields");
            }
            else if (!DoesUserHaveAddressDetails(user))
            {
                return new ActionResponse(false, 400, "You have to add address details, in order to update specific fields");
            }
            else
            {
                return new ActionResponse(true);
            }
        }
    }
}
