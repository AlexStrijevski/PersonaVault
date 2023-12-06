using PersonaVault.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.DTOs
{
    public class FullUserData
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartamentNumber { get; set; }
    }
}
