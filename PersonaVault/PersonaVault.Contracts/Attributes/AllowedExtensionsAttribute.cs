using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if(extension != null)
                {
                    return new ValidationResult("Cannot accept files with no extensions");
                }
                if (!_extensions.Contains(extension.ToUpper()))
                {
                    return new ValidationResult("Unsupported file extension");
                }
            }
            return ValidationResult.Success;
        }
    }
}
