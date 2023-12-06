using Microsoft.EntityFrameworkCore;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Repositories
{
    internal class PersonalDetailsRepository : IPersonalDetailsRepository
    {
        private readonly PersonaVaultContext _context;

        public PersonalDetailsRepository(PersonaVaultContext context)
        {
            _context = context;
        }

        public async Task CreateAndAddPersonalDetailsToUser(EncryptedPersonalDetailsDTO request, User user)
        {
            var personalDetails = new PersonalDetails
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LastName = request.LastName,
                PersonalCode = request.PersonalCode,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress,
                Picture = request.Picture
            };

            await CreatePersonalDetailsInDatabase(personalDetails);

            user.PersonalDetails = personalDetails;
            await _context.SaveChangesAsync();
        }

        private async Task CreatePersonalDetailsInDatabase(PersonalDetails personalDetails)
        {
            _context.PersonalDetails.Add(personalDetails);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateName(string newName, User user)
        {
            user.PersonalDetails.Name = newName; 
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLastName(string newLastName, User user)
        {
            user.PersonalDetails.LastName = newLastName;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePersonalCode(byte[] personalCode, User user)
        {
            user.PersonalDetails.PersonalCode = personalCode;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmail(byte[] email, User user)
        {
            user.PersonalDetails.EmailAddress = email;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhoneNumber(byte[] phoneNumber, User user)
        {
            user.PersonalDetails.PhoneNumber = phoneNumber;

            await _context.SaveChangesAsync();
        }
    }
}
