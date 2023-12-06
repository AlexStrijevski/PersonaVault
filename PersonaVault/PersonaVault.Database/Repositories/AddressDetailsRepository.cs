using PersonaVault.Contracts.DTOs;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Repositories
{
    internal class AddressDetailsRepository : IAddressDetailsRepository
    {
        private readonly PersonaVaultContext _context;

        public AddressDetailsRepository(PersonaVaultContext context)
        {
            _context = context;
        }

        public async Task CreateAddressDetailsAndAddToUser(EncryptedAddressDetailsDTO data, User user)
        {
            var personalDetails = new AddressDetails
            {
                Country = data.Country,
                City = data.City,
                Street = data.Street,
                HouseNumber = data.HouseNumber,
                ApartamentNumber = data.ApartamentNumber,
            };

            await CreateAddressDetailsInDatabase(personalDetails);

            user.PersonalDetails.AddressDetails = personalDetails;

            await _context.SaveChangesAsync();
        }

        private async Task CreateAddressDetailsInDatabase(AddressDetails addressDetails)
        {
            _context.AddressDetails.Add(addressDetails);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCountry(byte[] country, User user)
        {
            user.PersonalDetails.AddressDetails.Country = country;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCity(byte[] city, User user)
        {
            user.PersonalDetails.AddressDetails.City = city;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStreet(byte[] street, User user)
        {
            user.PersonalDetails.AddressDetails.Street = street;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateHouseNumber(byte[] houseNumber, User user)
        {
            user.PersonalDetails.AddressDetails.HouseNumber = houseNumber;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateApartamentNumber(byte[] apartamentNumber, User user)
        {
            user.PersonalDetails.AddressDetails.ApartamentNumber = apartamentNumber;

            await _context.SaveChangesAsync();
        }
    }
}
