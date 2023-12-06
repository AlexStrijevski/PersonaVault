using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonaVault.Business.Requirements
{
    internal class PersonalDetailsRequirements : IPersonalDetailsRequirements
    {
        public bool DoesPersonalDetailsMeetRequirements(NewPersonalDetailsDTO data)
        {
            if (DoesEmailMeetRequirements(data.EmailAddress) &&
                DoesPersonalCodeMeetRequirements(long.Parse(data.PersonalCode)) &&
                DoesPhoneNumberMeetRequirements(long.Parse(data.PhoneNumber)))
            {
                return true;
            }
            return false;
        }

        public bool DoesEmailMeetRequirements(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public bool DoesPersonalCodeMeetRequirements(long personalCode)
        {
            if (personalCode > 30000000000 && personalCode < 50000000000) return true;
            return false;
        }

        public bool DoesPhoneNumberMeetRequirements(long phoneNumber)
        {
            if (phoneNumber > 37050000000 && phoneNumber < 37070000000) return true;
            return false;
        }
    }
}
