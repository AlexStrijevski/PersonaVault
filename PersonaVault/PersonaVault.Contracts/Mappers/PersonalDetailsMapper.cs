using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Mappers
{
    internal class PersonalDetailsMapper : IPersonalDetailsMapper
    {
        public NewPersonalDetailsDTO MapToPersonalDetailsDTO(CreatePersonalDetailsRequest request, byte[] picture)
        {
            return new NewPersonalDetailsDTO
            {
                Name = request.Name,
                LastName = request.LastName,
                PersonalCode = request.PersonalCode,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress,
                Picture = picture
            };
        }
    }
}
