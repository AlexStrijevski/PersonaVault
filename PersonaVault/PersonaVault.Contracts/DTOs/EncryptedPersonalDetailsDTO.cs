using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.DTOs
{
    public class EncryptedPersonalDetailsDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public byte[] PersonalCode { get; set; }
        public byte[] PhoneNumber { get; set; }
        public byte[] EmailAddress { get; set; }
        public byte[] Picture { get; set; }
    }
}
