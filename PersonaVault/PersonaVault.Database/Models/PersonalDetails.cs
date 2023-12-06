using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Models
{
    public class PersonalDetails
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName{ get; set; }
        public byte[] PersonalCode { get; set; }
        public byte[] PhoneNumber { get; set; }
        public byte[] EmailAddress { get; set; }
        public byte[] Picture { get; set; }
        public Guid? AddressDetailsId { get; set; }
        public AddressDetails? AddressDetails { get; set; }
    }
}
