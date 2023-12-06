using Microsoft.AspNetCore.Http;
using PersonaVault.Contracts.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Requests
{
    public class CreatePersonalDetailsRequest
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Personal Code cannot be empty")]
        public string PersonalCode { get; set; }

        [Required(ErrorMessage = "You have upload a picture")]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(".JPG", ".JPEG", ".GIF", ".TIFF", ".BMP", ".RAW", ".SVG", ".WEBP", ".HEIF", ".PSD")]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = "Phone Number cannot be empty")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        public string EmailAddress { get; set; }
    }
}
