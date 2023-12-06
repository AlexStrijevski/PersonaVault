using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Requests
{
    public class CreateAddressDetailsRequest
    {
        [Required(ErrorMessage = "Country cannot be empty")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City cannot be empty")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street cannot be empty")]
        public string Street { get; set; }

        [Required(ErrorMessage = "House Number cannot be empty")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Apartament Number cannot be empty")]
        public string ApartamentNumber { get; set; }
    }
}
