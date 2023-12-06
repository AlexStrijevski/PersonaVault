using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.DTOs
{
    public class NewPersonalDetailsDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public byte[] Picture { get; set; }
    }
}
