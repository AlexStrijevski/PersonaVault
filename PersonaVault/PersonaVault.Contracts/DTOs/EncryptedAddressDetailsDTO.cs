using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.DTOs
{
    public class EncryptedAddressDetailsDTO
    {
        public byte[] Country { get; set; }

        public byte[] City { get; set; }

        public byte[] Street { get; set; }

        public byte[] HouseNumber { get; set; }

        public byte[] ApartamentNumber { get; set; }
    }
}
